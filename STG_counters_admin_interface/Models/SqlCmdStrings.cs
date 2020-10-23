using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG_counters_admin_interface.Models
{
	public static class SqlCmdStrings
	{
		static public string Sql_cmd_ExistPlacesIDandNames(string PlacesNamesId) =>
			@"select ROW_NUMBER() OVER(ORDER BY  PlaceID ASC) as 'ID', * from(
			select PlaceID, name as 'PlaceName' from vPlaces
			union
			select PlaceID, PlaceName from STG_PlacesNames
			where PlacesNamesId<>" + PlacesNamesId + " ) a";

		static public string Sql_cmd_UpdateDevicePlacesNames(string PlacesNamesId) =>
			@"select ROW_NUMBER() OVER(ORDER BY  PlaceID ASC) as 'ID', * from(
					select PlaceID, name as 'PlaceName' from vPlaces
					union
					select PlaceID, PlaceName from STG_PlacesNames
					where PlacesNamesId<>" + PlacesNamesId + " ) a";

		static public string Sql_cmd_Plan_TER_Editing(int year, string sku, int PlaceId)=>
			@"

 declare @year int
 set @year ="
+ year.ToString()
+ @"
declare @PlaceId int
 set @PlaceId = "
+ PlaceId
+ @"
declare @Sku NVARCHAR(MAX)
 set @Sku = '"
+ sku + @"'

 
 DECLARE @Mounths TABLE
(
  mounth int  
)

 declare @month int
 set @month = 1
 while(@month<13)
 begin
 insert into @Mounths
 values (@month)
 set @month=@month+1
 end


SELECT m.mounth  as [Id_STG_Plan_TER]
      , @PlaceId as [PlaceId]
	  , @Sku as [Sku]
      , @year as [Year]
      ,m.mounth as Month
	  ,isnull([BO_Value],0) as [BO_Value]
      ,isnull([Shift_productivity_min],0) as [Shift_productivity_min]
      ,isnull([Shift_productivity_norm],0) as [Shift_productivity_norm]
      
      ,isnull([Energy_perTonne_min],0) as [Energy_perTonne_min]
      ,isnull([Energy_perTonne_norm],0) as [Energy_perTonne_norm]
      ,isnull([Steam_perTonne_min],0) as [Steam_perTonne_min]
      ,isnull([Steam_perTonne_norm],0) as [Steam_perTonne_norm]
      ,isnull([Gas_perTonne_min],0) as [Gas_perTonne_min]
      ,isnull([Gas_perTonne_norm],0) as [Gas_perTonne_norm]
      ,isnull([Avarage_speed_min],0) as [Avarage_speed_min]
      ,isnull([Avarage_speed_norm],0) as [Avarage_speed_norm]
      
  FROM [dbo].[STG_Plan_TER] sku
right join @Mounths m on sku.Month = m.mounth and @Sku = sku.Sku and @year = sku.Year and @PlaceId=sku.PlaceId 


";
		static public string avg_speed_over_csp_by_hours = @"DECLARE @avgSpeed table( startPeriod DateTime
, endPeriod DateTime, AverageSpeed float);

declare @tg table (Date DateTime, value float ) 

insert into @tg
select date, value from STG_Cache_TagDump tg
where @startDate<=Date and Date<=@endDate and TagID = @TagId
order by tagid,date
	
insert into @avgSpeed 
	select 
	d.startPeriod, d.endPeriod, round(AVG(td.Value),0) 
	from @Dates d
left join (select Date, value from @tg ) td on td.Date between d.startPeriod and d.endPeriod
group by d.startPeriod,d.endPeriod";

		static public string csp_shifts_fluid_stream_and_wetness = $@"
declare @Dates table(startPeriod DateTime, endPeriod DateTime)

declare @currDate DateTime
set @currDate=@startDate
while @currDate<@endDate
begin
insert into @Dates values(@currDate,dateadd(MINUTE,@diff,@currDate))
set @currDate=dateadd(MINUTE,@diff,@currDate)
end




	declare @csp_fluid_stream table(
	[Id] [int]  NOT NULL,	
	[FluidStream] [float] NULL)
	
	declare @csp_wetness table(
	[Id] [int]  NOT NULL,	
	[Wetness] [float] NULL)

	declare @mass table (startPeriod DateTime , endPeriod DateTime, mass float) 
declare @conentr table (startPeriod DateTime , endPeriod DateTime, concentr float) 

declare @TagDump table([TagId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Value] [float])

insert @TagDump
select TagId,Date,Value from
STG_Cache_TagDump td
where  @startDate<=date and date<=@endDate
and TagID in (4,7,70,78)
order by TagId,Date
--select * from @Dates
--select * from @TagDump

insert into @mass
select d.*, sum(
case when value>=550
then value
when value<550
then 0 end
) as mass  from @Dates d
left join @TagDump ao01 on (startPeriod<= ao01.Date and ao01.Date <=endPeriod) and ao01.TagId = 4
group by d.startPeriod,d.endPeriod

insert into @conentr
select d.*, avg(case when value>=0.5
then value
when value<0.5
then 0 end )/100 as concentr  from @Dates d
join @TagDump ao01 on startPeriod<= ao01.Date and ao01.Date <=endPeriod and ao01.TagId = 7
group by d.startPeriod,d.endPeriod

declare @fluid_stream table (startPeriod DateTime, endPeriod DateTime, fluid_stream float)

insert into @fluid_stream
select dates.*, mass.mass*conentr.concentr as fluid_stream from @Dates dates -- считаем житкий поток в периодах по @diff минут

join -- считаем массу в периодах по @diff минут
@mass mass on mass.startPeriod= dates.startPeriod and mass.endPeriod = dates.endPeriod

join -- считаем концентрацию в периодах по @diff минут
@conentr conentr on conentr.startPeriod= dates.startPeriod and conentr.endPeriod = dates.endPeriod

insert into @csp_fluid_stream  -- считаем жидкий поток по циклам
SELECT [ID]    
      ,sum (fluid_stream) as [FluidStream]
	  
	  
	  
  FROM @csp csp --добавляем житкий поток распределенный по циклам производства
join @fluid_stream  

 
 fluid_stream_select on csp.CycleDateBegin <= fluid_stream_select.startPeriod  and fluid_stream_select.endPeriod<=csp.CycleDateEnd



group by [Id]
      


declare @tag78 table(wetness float, Date DateTime)

insert @tag78 select 
 value as wetness, Date
from @TagDump td  where  TagId=78
--select * from @tag78
declare @tag70 table( wetness float, Date DateTime)

insert @tag70 select
 value as wetness, Date
from @TagDump td  where  TagId=70
--select * from @tag70

--select * from @csp

insert into @csp_wetness  -- считаем влажность по циклам
	  SELECT 
      Id,avg(td_wetness_bdm1.wetness) as wetness
	  
  FROM @csp csp --добавляем житкий поток распределенный по циклам производства
  
 
left join -- добавляем влажность из бдм-1 
@tag78 td_wetness_bdm1 on  csp.CycleDateBegin <=td_wetness_bdm1.Date and  td_wetness_bdm1.Date<=csp.CycleDateEnd and PlaceID=1


where @startDate<=csp.CycleDateBegin and csp.CycleDateEnd<=@endDate and PlaceID=1
group by Id
union
 SELECT 
      Id,avg(td_wetness_bdm2.wetness) as wetness
	  
  FROM @csp csp --добавляем житкий поток распределенный по циклам производства
  
 
left join -- добавляем влажность из бдм-2 
@tag70 td_wetness_bdm2 on  csp.CycleDateBegin <=td_wetness_bdm2.Date and  td_wetness_bdm2.Date<=csp.CycleDateEnd and PlaceID=2



where @startDate<=csp.CycleDateBegin and csp.CycleDateEnd<=@endDate and PlaceID=2
group by Id



--select * from @csp_wetness
SET NOCOUNT OFF
	  	  update STG_Cache_CycleSpoolProduction_byShifts --записываем вычисленные жидкий поток  в исходик
set FluidStream = csp_fluid_stream.FluidStream
from STG_Cache_CycleSpoolProduction_byShifts csp
left join @csp_fluid_stream csp_fluid_stream on csp.Id = csp_fluid_stream.Id
where csp.Id = csp_fluid_stream.Id

 update STG_Cache_CycleSpoolProduction_byShifts --записываем вычисленные  влажность в исходик
set  Wetness = csp_wetness.Wetness
from STG_Cache_CycleSpoolProduction_byShifts csp
left join @csp_wetness csp_wetness on csp.Id = csp_wetness.Id
where 	csp.Id = csp_wetness.Id   

--update 
";
		static public string Fill_Cache_CycleSpoolProduction_byShifts(DateTime start_Date, DateTime end_Date)
		{ 
			
			string sql_query_string = $@"
declare @cps_import_min_date datetime
declare @cps_import_max_date datetime

set @cps_import_min_date = {StaticMethods.SQL_DateTime(start_Date)}
set @cps_import_max_date = {StaticMethods.SQL_DateTime(end_Date)}
delete from [dbo].[STG_Cache_CycleSpoolProduction_byShifts]
where CycleDateBegin>=@cps_import_min_date and CycleDateEnd<=@cps_import_max_date
DECLARE @csp table([Cycle] int
      
      ,[CycleDateBegin] Datetime
      ,[CycleDateEnd] Datetime
	  ,[Place] nvarchar(max)
      ,[Weight] float
      ,[PlaceID] int
      ,[Composition] nvarchar(max)
	  ,[ShiftID] int)

	insert into @csp  SELECT Cycle, MIN(Date) AS [CycleDateBegin],  MAX(Date) AS [CycleDateEnd],
       --SpoolKind, 	  
			 Place, SUM(Weight) *1000 AS Weight, a.PlaceID, a.Composition, a.ShiftID
FROM (
		SELECT ROW_NUMBER() OVER (ORDER BY d.PlaceID, d.Date) - RANK() 
							OVER (PARTITION BY d.PlaceID, nsp.SortValue + ' ' + nsp.Composition + ' ' + nsp.ColorGroup, d.ShiftID ORDER BY d.Date) AS [Cycle],DAY(d.Date) as day, d.Date, n.Name AS [SpoolKind], 
				p.Name AS [Place], dpp.Quantity AS [Weight], d.PlaceID,
				nsp.SortValue, 
				nsp.SortValue + ' ' + nsp.Composition + ' ' + nsp.ColorGroup AS Composition

				, nsp.RawMaterial
				, d.ShiftID
		FROM [GAMMA].[GammaNew].[dbo].[ProductSpools] ps 
		JOIN [GAMMA].[GammaNew].[dbo].[1CNomenclature] n ON n.[1CNomenclatureID] = ps.[1CNomenclatureID]
		JOIN [GAMMA].[GammaNew].[dbo].[1CCharacteristics] c ON ps.[1CCharacteristicID] = c.[1CCharacteristicID]
		JOIN [GAMMA].[GammaNew].[dbo].[DocProductionProducts] dpp ON dpp.ProductID = ps.ProductID
		JOIN [GAMMA].[GammaNew].[dbo].[Docs] d ON d.DocID = dpp.DocID
		JOIN [GAMMA].[GammaNew].[dbo].[Places] p ON p.PlaceID = d.PlaceID AND (d.PlaceID IN (1, 2))
		JOIN [GAMMA].[GammaNew].[dbo].[vNomenclatureSGBProperties] nsp ON nsp.[1CNomenclatureID] = n.[1CNomenclatureID]
		WHERE d.Date >= @cps_import_min_date and d.Date <= @cps_import_max_date) a
GROUP BY Cycle, Place, PlaceID, a.SortValue,a.Composition, a.RawMaterial, a.SortValue, a.ShiftID
DECLARE @avgSpeed table( PlaceId int, CycleDateBegin DateTime
, CycleDateEnd DateTime, cycle int,  avgSpeed float);
	
INSERT INTO [STG_Cache_CycleSpoolProduction_byShifts]
           ([Cycle]
           ,[CycleDateBegin]
		   ,[CycleDateEnd]           
           ,[Place]
           ,[Weight]
           ,[PlaceID]
           ,[Composition]
		   ,[ShiftID]
		   ,[Machinist_name]
           ,[AverageSpeed]
		   )
		   
(
select distinct csp.*, vpi.PrintName as Machinist_name, --isnull(c.AvgSpeed,0) 
CAST(0 as float) as AverageSpeed
from @csp csp
--LEFT JOIN @avgSpeed c ON c.CycleDateBegin = csp.CycleDateBegin and c.CycleDateEnd = csp.CycleDateEnd
left JOIN (SELECT DISTINCT PlaceID, PrintName, ShiftID, Date FROM Gamma.GammaNew.dbo.vProductsInfo
		WHERE PlaceID IN (1,2) AND Date >= @cps_import_min_date AND Date <=  @cps_import_max_date) vpi on vpi.PlaceID= csp.PlaceID and vpi.ShiftID=csp.ShiftID and vpi.Date >= csp.CycleDateBegin AND vpi.Date <=  csp.CycleDateEnd
)
";
		return sql_query_string;
		}

		static public string Fill_Cache_CycleSpoolProduction(DateTime start_Date,DateTime end_Date, bool isDay)
		{
			//isDay==true - генерируем запрос для разрезания цикла на дни иначе на месяцы
			string sql_query_string = $@"delete from STG_Cache_CycleSpoolProduction_by"+(isDay?"Day":"Month")+$@" 
where {StaticMethods.Filter_by_time_byPeriod_withTime(start_Date,end_Date, "CycleDateBegin", "CycleDateEnd")} "
+ @" 
DECLARE @csp table([Cycle] int
      ,[CycleDateEnd] Datetime
      ,[CycleDateBegin] Datetime
      ,[Place] nvarchar(max)
      ,[Weight] float
      ,[PlaceID] int
      ,[Composition] nvarchar(max)"
+ (isDay ? ",[day] int" : "")
	  + @",[month] int
	   ,[year] int);
	insert into @csp "
+ @" SELECT Cycle, MAX(Date) AS [CycleDateEnd], MIN(Date) AS [CycleDateBegin], 
       --SpoolKind, 	  
			 Place, SUM(Weight) *1000 AS Weight, a.PlaceID, a.Composition,"
+ (isDay ? "day," : "")
+ end_Date.Month.ToString()
+ @"  as month,"
+ end_Date.Year.ToString()
+ @" as year
FROM (
		SELECT ROW_NUMBER() OVER (ORDER BY d.PlaceID, d.Date) - RANK() 
							OVER (PARTITION BY d.PlaceID, nsp.SortValue + ' ' + nsp.Composition + ' ' + nsp.ColorGroup ORDER BY d.Date) AS [Cycle],"
		 + ( isDay?"DAY(d.Date) as day,":"")				
		+@" d.Date, n.Name AS [SpoolKind], 
				p.Name AS [Place], dpp.Quantity AS [Weight], d.PlaceID,
				nsp.SortValue, 
				nsp.SortValue + ' ' + nsp.Composition + ' ' + nsp.ColorGroup AS Composition

				, nsp.RawMaterial
		FROM [GAMMA].[GammaNew].[dbo].[ProductSpools] ps 
		JOIN [GAMMA].[GammaNew].[dbo].[1CNomenclature] n ON n.[1CNomenclatureID] = ps.[1CNomenclatureID]
		JOIN [GAMMA].[GammaNew].[dbo].[1CCharacteristics] c ON ps.[1CCharacteristicID] = c.[1CCharacteristicID]
		JOIN [GAMMA].[GammaNew].[dbo].[DocProductionProducts] dpp ON dpp.ProductID = ps.ProductID
		JOIN [GAMMA].[GammaNew].[dbo].[Docs] d ON d.DocID = dpp.DocID
		JOIN [GAMMA].[GammaNew].[dbo].[Places] p ON p.PlaceID = d.PlaceID AND (d.PlaceID IN (1, 2))
		JOIN [GAMMA].[GammaNew].[dbo].[vNomenclatureSGBProperties] nsp ON nsp.[1CNomenclatureID] = n.[1CNomenclatureID]
		WHERE "
+ StaticMethods.Filter_by_time(start_Date, end_Date, "d.Date")
		+ @") a
GROUP BY Cycle, Place, PlaceID, a.SortValue,"
+ (isDay? "a.day,":"")
+@"a.Composition, a.RawMaterial, a.SortValue
"


+ @"
INSERT INTO [STG_Cache_CycleSpoolProduction_by"+(isDay?"Day":"Month")+@"]
           ([Cycle]
           ,[CycleDateEnd]
           ,[CycleDateBegin]
           ,[Place]
           ,[Weight]
           ,[PlaceID]
           ,[Composition]
"
		   + (isDay?",[day]":"")
		   + @"
           ,[month]
           ,[year]
           ,[AverageSpeed])
(select csp.*, Cast (0 as float) as AverageSpeed from @csp csp
--LEFT JOIN @avgSpeed c ON c.CycleDateBegin = csp.CycleDateBegin and c.CycleDateEnd = csp.CycleDateEnd
)
";
			return sql_query_string;
		}

		public static string clear_Cache_TagDump = @"delete from [dbo].[STG_Cache_TagDump] where 1=1";
		public static string fill_Cache_TagDump = @"insert into [dbo].[STG_Cache_TagDump]
SELECT [TagId]
      ,[Date]
      ,[Value]
      ,[period]
  FROM [AO01].[gammaarch].[dbo].[TagDump] 

where (TagId in (4, 7, 29, 30, 70, 78) ) --average speed( 29 for BDM-1, 30 for BDM-2);humidity(78 for BDM-1, 70 for BDM-2); mass expense (4 for STG); concsntration (7 for STG)
";

		public static string TER_avg_permonth_part1 = @"
insert into [dbo].[STG_Cache_TERperMonths]
Select mc.SKU
,mc.PlaceID
,mc.PlaceName

,mc.year,";
		public static string TER_avg_permonth_part2 = @"
, ROUND(Sum(mc.mValueOfBO)/1000,3) as ValueOfBO
, ROUND(Sum(EnegryConsumption)/(Sum(mc.mValueOfBO)/1000),3) as EnegryConsumptionperTonne
, ROUND(Sum(SteamConsumption)/(Sum(mc.mValueOfBO)/1000),3) as SteamConsumptionperTonne
, ROUND(Sum(GasConsumption)/(Sum(mc.mValueOfBO)/1000),3) as GasConsumptionperTonne
, ROUND(avg(AverageSpeed),0) as AverageSpeed
 from (
SELECT
	  b.PlaceID , 
	  b.Place as PlaceName ,
	  b.Composition AS SKU,
	 b.month,
	 b.year,
	
	  b.Weight as mValueOfBO, 
	  
	      (SUM(isnull(a.Amount,0)))/1000
	   AS EnegryConsumption,
	  
	     (SUM(isnull(a.GasValue,0))/1000)
	   AS GasConsumption,
	  	  
	  (SUM(isnull(a.SteamValue,0)))
	    AS SteamConsumption,
	  
	   avg(isnull(AverageSpeed,0)) AS AverageSpeed	  
	FROM
	 @csp b 
	left JOIN ( 
	  SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date
	  FROM [ElectricEnergyPlacesHourly] a
	  --Это период месяц плюс день в начале чтобы если начала цикла чуть выходит наз границу месяца, то по нему данные бы попадали
	  
	  WHERE 1=1 ";

public static string TER_avg_permonth_part3 = @"
	  ) a ON b.PlaceID = a.PlaceID AND a.Date BETWEEN b.CycleDateBegin AND b.CycleDateEnd
	

	GROUP BY b.year,b.Place, b.PlaceID, /*b.Sort,*/

	Weight, b.Composition, b.month	
	HAVING  SUM(a.Amount)/1000 > 0) mc
	group by
 mc.SKU
,mc.year
";
		public static string TER_avg_permonth_part4 = @"
,mc.PlaceID
,mc.PlaceName

order by SKU
";
	public static string clear_STG_Cache_TERperMonths = @"
						delete from [dbo].[STG_Cache_TERperMonths]
						where 1=1 ";
	public static string csp_by_month = @"
	DECLARE @csp table([Id] int
      ,[Cycle] int
      ,[CycleDateEnd] Datetime
      ,[CycleDateBegin] Datetime
      ,[Place] nvarchar(max)
      ,[Weight] float
      ,[PlaceID] int
      ,[Composition] nvarchar(max)
      ,[month] int 
      ,[year] int
	  ,[AverageSpeed] float);
	insert into @csp
	select * from STG_Cache_CycleSpoolProduction_byMonth csp
	WHERE 1=1";
		public static string csp_by_shifts = @"
	DECLARE @csp table([Id] int
      ,[Cycle] int      
      ,[CycleDateBegin] Datetime
	  ,[CycleDateEnd] Datetime		
      ,[Place] nvarchar(max)
      ,[Weight] float
      ,[PlaceID] int
      ,[Composition] nvarchar(max)
      ,[ShiftID] int
	  ,[Machinist_name] nvarchar(max)
	  ,[FluidStream] [float] NULL
	  ,[Wetness] [float] NULL
	  ,[AverageSpeed] float);
	insert into @csp
	select * from STG_Cache_CycleSpoolProduction_byShifts csp
	WHERE 1=1";
		public static string csp_by_day = @"
	DECLARE @csp table([Id] int
      ,[Cycle] int
      ,[CycleDateEnd] Datetime
      ,[CycleDateBegin] Datetime
      ,[Place] nvarchar(max)
      ,[Weight] float
      ,[PlaceID] int
      ,[Composition] nvarchar(max)
	  ,[day] int      
	  ,[month] int 
      ,[year] int
	  ,[AverageSpeed] float);
	insert into @csp
	select * from STG_Cache_CycleSpoolProduction_byDay csp
	WHERE 1=1";
		public static string avg_speed_over_csp_by_month = @"
	DECLARE @avgSpeed table( PlaceId int, year int, month int, cycle int,  avgSpeed float);
	insert into @avgSpeed
	SELECT 
  csp.PlaceID	

, csp.year
, csp.month
, csp.Cycle
, ROUND(AVG(td.Value),0) AS [AvgSpeed]
	FROM @csp csp
		JOIN
		STG_Cache_TagDump td ON 
			((td.TagID = 30 AND csp.PlaceID = 2) OR (td.TagID = 29 AND csp.PlaceID = 1)) 
			AND (td.Date BETWEEN csp.CycleDateBegin AND csp.CycleDateEnd)
	GROUP BY csp.Cycle, csp.year, csp.month, csp.PlaceID
    order by csp.PlaceID, year, month, Cycle
	";

		public static string avg_speed_over_csp_by_shifts = @"DECLARE @avgSpeed table( csp_id int, PlaceId int, CycleDateBegin DateTime
, CycleDateEnd DateTime,  avgSpeed float);
	insert into @avgSpeed
	SELECT 
  csp.Id as csp_id,
  csp.PlaceID	

, csp.CycleDateBegin
, csp.CycleDateEnd

, ROUND(AVG(td.Value),0) AS [AvgSpeed]
	FROM @csp csp
		JOIN
		STG_Cache_TagDump td ON 
			((td.TagID = 30 AND csp.PlaceID = 2) OR (td.TagID = 29 AND csp.PlaceID = 1)) 
			AND (td.Date BETWEEN csp.CycleDateBegin AND csp.CycleDateEnd)
	GROUP BY csp.Id,  csp.CycleDateBegin
, csp.CycleDateEnd, csp.PlaceID
    order by csp.PlaceID, csp.CycleDateBegin
, csp.CycleDateEnd";
		public static string avg_speed_over_csp = @"DECLARE @avgSpeed table( csp_id int, PlaceId int, CycleDateBegin DateTime
, CycleDateEnd DateTime,  avgSpeed float);
	insert into @avgSpeed
	SELECT 
  csp.Id as csp_id,
  csp.PlaceID	

, csp.CycleDateBegin
, csp.CycleDateEnd

, ROUND(AVG(td.Value),0) AS [AvgSpeed]
	FROM @csp csp
		JOIN
		STG_Cache_TagDump td ON 
			((td.TagID = 30 AND csp.PlaceID = 2) OR (td.TagID = 29 AND csp.PlaceID = 1)) 
			AND (td.Date BETWEEN csp.CycleDateBegin AND csp.CycleDateEnd)
	GROUP BY csp.Id,  csp.CycleDateBegin
, csp.CycleDateEnd, csp.PlaceID
    order by csp.PlaceID, csp.CycleDateBegin
, csp.CycleDateEnd";
		public static string avg_speed_over_csp_by_cycles = @"DECLARE @avgSpeed table( PlaceId int, CycleDateBegin DateTime
, CycleDateEnd DateTime, cycle int,  avgSpeed float);
	insert into @avgSpeed
	SELECT 
  csp.PlaceID	

, csp.CycleDateBegin
, csp.CycleDateEnd
, csp.Cycle
, ROUND(AVG(td.Value),0) AS [AvgSpeed]
	FROM @csp csp
		JOIN
		STG_Cache_TagDump td ON 
			((td.TagID = 30 AND csp.PlaceID = 2) OR (td.TagID = 29 AND csp.PlaceID = 1)) 
			AND (td.Date BETWEEN csp.CycleDateBegin AND csp.CycleDateEnd)
	GROUP BY csp.Cycle,  csp.CycleDateBegin
, csp.CycleDateEnd, csp.PlaceID
    order by csp.PlaceID, csp.CycleDateBegin
, csp.CycleDateEnd, Cycle";


		public static string Update_csp_avg_speed(string table_name) => $@"
UPDATE {table_name} 
SET AverageSpeed = avgS.avgSpeed
FROM {table_name} csp_out
join @avgSpeed avgS on avgS.csp_id = csp_out.Id";

		public static string Update_csp_byShifts_avg_speed = @"
UPDATE STG_Cache_CycleSpoolProduction_byShifts 
SET AverageSpeed = avgS.avgSpeed
FROM STG_Cache_CycleSpoolProduction_byShifts csp_out
join @avgSpeed avgS on avgS.csp_id = csp_out.Id";

		public static string avg_speed_over_csp_by_cycles_ao01 = @"DECLARE @avgSpeed table( PlaceId int, CycleDateBegin DateTime
, CycleDateEnd DateTime, cycle int,  avgSpeed float);
	insert into @avgSpeed
	SELECT 
  csp.PlaceID	

, csp.CycleDateBegin
, csp.CycleDateEnd
, csp.Cycle
, ROUND(AVG(td.Value),0) AS [AverageSpeed]
	FROM @csp csp
		JOIN
		[AO01].[gammaarch].[dbo].[TagDump] td ON 
			((td.TagID = 30 AND csp.PlaceID = 2) OR (td.TagID = 29 AND csp.PlaceID = 1)) 
			AND (td.Date BETWEEN csp.CycleDateBegin AND csp.CycleDateEnd)
	GROUP BY csp.Cycle,  csp.CycleDateBegin
, csp.CycleDateEnd, csp.PlaceID
    order by csp.PlaceID, csp.CycleDateBegin
, csp.CycleDateEnd, Cycle";

		public static string ConfigurationsController_Places_list = @"select ROW_NUMBER() OVER(ORDER BY  PlaceID ASC) as 'ID', * from(
			select PlaceID, name as 'PlaceName' from vPlaces
			union
			select PlaceID, PlaceName from STG_PlacesNames
			) a";

		public static string ReportController_ConsumptionByCycle_part1 = @"
--DECLARE @DateBegin datetime, @DateEnd datetime
--SET @DateBegin = '20190101'
--SET @DateEnd = '20190110'
--SET @DateEnd = GETDATE()

    SELECT
     ROW_NUMBER() OVER(ORDER BY b.CycleDateBegin ASC) AS 'ID',
b.CycleDateBegin ,     
b.CycleDateEnd,
b.Place , 
	  b.Composition AS [SortofProduction],


     --CAST(b.Value as VARCHAR(5)) AS[Граммаж], 
	  
	  ROUND(SUM(isnull(a.Amount,0)) / 1000, 2) AS [EnergyConsumption], 
      isnull(b.Weight,0) / 1000 AS[CapacityOfBO],	  
CASE b.Composition        
WHEN 'Прочее' THEN 0  
      ELSE     ROUND((SUM(isnull(a.Amount,0))/(b.Weight/1000))/1000, 3)
	  end AS[EnergyConsumptionPerTonne],      
       
		ROUND(SUM(isnull(a.GasValue,0)) / 1000, 3) AS[GasConsumption],
	  SUM(isnull(a.SteamValue,0)) AS[SteamConsumption],
	  CASE b.Composition  
	  WHEN 'Прочее' THEN 0  
      ELSE     ROUND((SUM(isnull(a.GasValue,0)) / (b.Weight / 1000)) / 1000, 3)
	  end 
	   AS[GasConsumptionPerTonne],
CASE b.Composition 
WHEN 'Прочее' THEN 0  
      ELSE     ROUND((SUM(isnull(a.SteamValue,0)) / (b.Weight / 1000)), 3)
	  end 	  
 AS[SteamConsumptionPerTonne]
, ROUND(AVG(c.AvgSpeed),0) AS AverageSpeed

    FROM

  --  Gamma.GammaNew.dbo.CycleSpoolProduction 
";

		public static string ReportController_ConsumptionByCycle_part2 =	@" b

    left JOIN(
      SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date

      FROM PowerDB.dbo.[ElectricEnergyPlacesHourly] a
where ";
		
		public static string ReportController_ConsumptionByCycle_part3 = @") a ON b.PlaceID = a.PlaceID AND a.Date BETWEEN b.CycleDateBegin AND b.CycleDateEnd"; 
public static string ReportController_ConsumptionByCycle_part_where =@"    
WHERE b.PlaceID IN(1, 2) --AND  b.[Weight] / 1000 > 20";
		public static string ReportController_ConsumptionByCycle_avg_speed_join_part1 =
		@"LEFT JOIN 
	(SELECT csp.Cycle, csp.CycleDateBegin, csp.CycleDateEnd, csp.PlaceID, AVG(td.Value) AS [AvgSpeed]
	FROM STG_Cache_CycleSpoolProduction_byMonth csp
		JOIN
		STG_Cache_TagDump td ON 
			((td.TagID = 30 AND csp.PlaceID = 2) OR (td.TagID = 29 AND csp.PlaceID = 1)) 
			AND (td.Date BETWEEN csp.CycleDateBegin AND csp.CycleDateEnd)
	where ";
		public static string ReportController_ConsumptionByCycle_avg_speed_join_part2 =

	@" GROUP BY csp.Cycle, csp.CycleDateBegin, csp.CycleDateEnd, csp.PlaceID
	) c ON c.Cycle = b.Cycle ";
		public static string ReportController_ConsumptionByCycle_part4 = @"

    GROUP BY b.Place,b.Composition, b.CycleDateBegin,b.CycleDateEnd, Weight
    HAVING SUM(a.Amount) / 1000 > 0 
-- AND SUM(b.Weight)/ 1000 > 39

    ORDER BY Place, CycleDateBegin";

		public static string ShowRecordsByStatusByCriteria_part1 = @"declare @start_date DateTime  
declare @cur_date DateTime

--дата по которой будем искать пропущенный данные
declare @max_date DateTime
set @start_date = CONVERT(Datetime, '";

		public static string ShowRecordsByStatusByCriteria_part2 = @"', 120)
set @max_date = CONVERT(Datetime, '";

		public static string ShowRecordsByStatusByCriteria_part3 = @"', 120)

SELECT ROW_NUMBER() OVER(ORDER BY  
a.DEVICE_NAME,  f.RECORD_TIME
ASC) AS 'ID', isnull(e.PlaceName,'Место не указано') as  PlaceName, 
                         a.DEVICE_NAME,a.DEVICE_ID,isnull(f.ID_RECORD, 0) as ID_RECORD, 
						--Если пропущенное время есть то выводим его, иначе время записи
						(f.RECORD_TIME


						) as RECORD_TIME,
						
						--Если данных по прибору нет, заменияем нолями
						ISNULL(d.PARAM_VALUE, 0) as PARAM_VALUE, ISNULL(d.MEASURE_VALUE, 0) as MEASURE_VALUE,
						
						 case
						 when ID_Criteria is not NULL then 'Не походит по критерию'
						-- when f.RECORD_TIME is NULL then 'Запись отсутствует'
				   --   when f.RECORD_TIME is not NULL  then 'Запись нормальная'


						  end as RecordStatus
					FROM PA_RECORDS f
					join PA_DEVICES a on f.ID_DEVICE = a.DEVICE_ID
					--ограничиваем по часовому или получасовму срезу в зависимости от итпа счетчика
					JOIN PA_ADAPTERS b ON b.ID_DEVICE = a.DEVICE_ID AND b.ADAPTER_LOGICAL_ID = 0
					--ограничиваем по считываемому каналу
					JOIN PA_ADAPTER_PARAMETERS c ON c.ID_ADAPTER = b.ID_ADAPTER AND a.DEVICE_ID = c.ID_DEVICE

					 AND(
					   (a.DEVICE_TYPE_NAME = 'Elster A1800 (Электросчетчик)' AND c.PARAMETER_NAME = 'Канал 1')

					   OR((a.DEVICE_TYPE_NAME = 'СЭТ-4TM.03 (Электросчетчик)' OR a.DEVICE_TYPE_NAME = 'Меркурий 230ART (Электросчетчик)') AND c.PARAMETER_NAME = 'A+ (Энергия активная +)')
						OR (a.DEVICE_TYPE_NAME = 'СПГ761 (газовый корректор)' AND c.PARAMETER_NAME = 'Объем газа при ст. усл., тп1')
						OR (a.DEVICE_TYPE_NAME = 'СПТ961.1(.2) (тепловычислитель)' AND c.PARAMETER_NAME ='Тепловая энергия, тп1')					
)
                      --список записей нужен либо для проверки отсутсвия записи, либо для вывода данных записи



					  --добавлем так же данные по записи

					left JOIN PA_DATA d ON d.ID_RECORD = f.ID_RECORD AND d.ID_PARAMETER = c.ID_PARAMETER and f.ID_ADAPTER = b.ID_ADAPTER";
		 
		 public static string ShowRecordsByStatusByCriteria_part4 = @"--место может не забито так что left join
		 left join DevicePlaces e on e.DEVICE_ID = a.DEVICE_ID and
		--учитываем промежутки расположения приборов в указаных местах
		e.BeginDateOfLocation <= f.RECORD_TIME and
		 e.EndDateOfLocation >= f.RECORD_TIME
		and e.Multiplier>0

		 where
		 f.RECORD_TIME >= @start_date and
		  f.RECORD_TIME <= @max_date and
			--f.ID_ADAPTER = b.ID_ADAPTER
		   (
				--не пустой ID_Criteria означает что есть критерий по которому запись не подходит
  
					  ID_Criteria is not NULL or
					  --пустой f.RECORD_TIME означает что записи нет
  
					  f.RECORD_TIME is NULL)
					   --Если место пусто, значит прибор не стоял
				--	 and PlaceName is not NULL";

		public static string ShowRecordsByStatus_absent_rec_part1 = @"set nocount on; declare  @Time_Set_Hour as TABLE (

    [RECORD_TIME]    DATETIME NOT NULL
);

declare  @Time_Set_HalfHour as TABLE (

    [RECORD_TIME]    DATETIME NOT NULL
);
declare  @Time_And_Devices_Set as TABLE (
    [ID] INT      IDENTITY (1, 1) NOT NULL,
    [ID_DEVICE]      INT      NOT NULL,
    [RECORD_TIME]    DATETIME NOT NULL
);


declare  @Time_And_Devices_Set_HalfHour as TABLE (
    [ID] INT      IDENTITY (1, 1) NOT NULL,
    [ID_DEVICE]      INT      NOT NULL,
    [RECORD_TIME]    DATETIME NOT NULL
);



declare  @Time_And_Devices_Set_Full as TABLE (
    [ID] INT      IDENTITY (1, 1) NOT NULL,
    [ID_DEVICE]      INT      NOT NULL,
    [RECORD_TIME]    DATETIME NOT NULL
);

declare @start_date DateTime  
declare @cur_date DateTime  
--дата по которой будем искать пропущенный данные
declare @max_date DateTime

set @start_date = CONVERT(Datetime, '";

		public static string ShowRecordsByStatus_absent_rec_part2 = @"', 120)
set @max_date = CONVERT(Datetime, '";

		
		public static string ShowRecordsByStatus_absent_rec_part_device_all =
			$@"', 120)

--заполняем значениям с периодом в час для нужного устройства
set @cur_date = @start_date
----------конец подготовки

while (@cur_date<@max_date)
begin

insert @Time_Set_Hour values (@cur_date)

set @cur_date = dateadd(HOUR, 1, @cur_date) 
end
set @cur_date = @start_date
while (@cur_date<@max_date)
begin

insert @Time_Set_HalfHour values (@cur_date)

set @cur_date = dateadd(MINUTE, 30, @cur_date) 
end
			--Создаем список устройств со всеми возможными временами в заданном периоде
					  insert
					  into @Time_And_Devices_Set
					  
					  select distinct  a.DEVICE_ID, b.RECORD_TIME 
					  
					   from (
--Собираем список устройств по которым будем искать пропущенные часовые данные 
SELECT       
                        DEVICE_ID--, f.RECORD_TIME --

                    FROM PA_DEVICES 
					where
                    DEVICE_TYPE_NAME in {StaticMethods.SqlInList(StaticMethods.DeviceTypesListwithHourlyIndication)}  
                   
					   
                       --JOIN PA_RECORDS f ON f.ID_ADAPTER = b.ID_ADAPTER AND f.ID_DEVICE = b.ID_DEVICE
					 --  where f.RECORD_TIME >= @cur_date and f.RECORD_TIME <= @max_date 
					   ) a
					  cross join ( select * from @Time_Set_Hour ) as b 
----------------------------------------------------------------
---Тоже для устройств с периодом в полчаса

  insert
					  into @Time_And_Devices_Set_HalfHour
					  
					  select distinct  a.DEVICE_ID, b.RECORD_TIME 
					  
					   from (
--Собираем список устройств по которым будем искать пропущенные для получаса


SELECT       
                        DEVICE_ID--, f.RECORD_TIME --

                    FROM PA_DEVICES 
					where
                   (DEVICE_TYPE_NAME in {StaticMethods.SqlInList(StaticMethods.DeviceTypesListwithHalfHourlyIndication)})
					   
                    
					   ) a
					  cross join ( select * from @Time_Set_HalfHour ) as b 
				
--Собираем полный список устройств(с часовыми и получасовыми показаниями) в одну таблицу
				insert
					  into @Time_And_Devices_Set_Full
					  select ID_DEVICE,RECORD_TIME from @Time_And_Devices_Set
					  union
					  select ID_DEVICE,RECORD_TIME from @Time_And_Devices_Set_HalfHour
		
----------------------------------------------------------------";

		public static string ShowRecordsByStatus_absent_rec_part_device_selected_type1_1 =
@"', 120)
--заполняем значениям с периодом в час для нужного устройства
set @cur_date = @start_date
----------конец подготовки

while (@cur_date<@max_date)
begin

insert @Time_Set_Hour values (@cur_date)

set @cur_date = dateadd(HOUR, 1, @cur_date) 
end
--Создаем список устройств со всеми возможными временами в заданном периоде
					  insert
					  into @Time_And_Devices_Set_Full
					  
					  select distinct  a.DEVICE_ID, b.RECORD_TIME 
					  
					   from (
--Собираем список устройств по которым будем искать пропущенные часовые данные 
SELECT       
                        DEVICE_ID--, f.RECORD_TIME --

                    FROM PA_DEVICES 
					where 1=1 
                    ";
		public static string ShowRecordsByStatus_absent_rec_part_device_selected_type1_2=@"
  ) a
					  cross join ( select * from @Time_Set_Hour ) as b ";


		public static string ShowRecordsByStatus_absent_rec_part_device_selected_type2_1 = $@"', 120)
--заполняем значениям с периодом в час для нужного устройства
set @cur_date = @start_date
----------конец подготовки
while (@cur_date<@max_date)
begin

insert @Time_Set_HalfHour values (@cur_date)

set @cur_date = dateadd(MINUTE, 30, @cur_date) 
end
----------------------------------------------------------------
---Тоже для устройств с периодом в полчаса

  insert
					  into @Time_And_Devices_Set_Full
					  
					  select distinct  a.DEVICE_ID, b.RECORD_TIME 
					  
					   from (
--Собираем список устройств по которым будем искать пропущенные для получаса


SELECT       
                        DEVICE_ID--, f.RECORD_TIME --

                    FROM PA_DEVICES 
					where 1=1 
                   
					   
";
		public static string ShowRecordsByStatus_absent_rec_part_device_selected_type2_2=
@"
					   ) a
					  cross join ( select * from @Time_Set_HalfHour ) as b ";
				

		public static string ShowRecordsByStatus_absent_rec_part4 = @"
--Берем список всех возможных дат в заданом диапазоне для каждого устройства и присоединяем к ним либо пустые данные если записей нет,
--либо данные с записи если таковые есть, так же помечаем в специальном столце recordStatus какие записи присоединили
					SELECT  ROW_NUMBER() OVER(ORDER BY e.PlaceName, a.DEVICE_NAME,  t.RECORD_TIME
ASC) AS 'ID', isnull(e.PlaceName,'Место не указано') as  PlaceName, 
                         a.DEVICE_NAME,a.DEVICE_ID,isnull(f.ID_RECORD,0) as ID_RECORD, 
						--Если пропущенное время есть то выводим его, иначе время записи
						COALESCE(t.RECORD_TIME,f.RECORD_TIME
						
						) as RECORD_TIME,
						
						--Если данных по прибору нет, заменияем нолями
						ISNULL(d.PARAM_VALUE,0) as PARAM_VALUE, ISNULL(d.MEASURE_VALUE,0) as MEASURE_VALUE,
						
						 case 
						-- when ID_Criteria is not NULL then 'Не походит по критерию'
						 when f.RECORD_TIME is NULL  then 'Запись отсутствует' 
						 when f.RECORD_TIME is not NULL  then 'Запись нормальная' 
						
						  end as RecordStatus
					FROM @Time_And_Devices_Set_Full t	   
                    join PA_DEVICES a on t.ID_DEVICE = a.DEVICE_ID
                    --ограничиваем по часовому или получасовму срезу в зависимости от итпа счетчика
					JOIN PA_ADAPTERS b ON b.ID_DEVICE = a.DEVICE_ID AND b.ADAPTER_LOGICAL_ID = 0
                    --ограничиваем  по считываемому каналу
					JOIN PA_ADAPTER_PARAMETERS c ON c.ID_ADAPTER = b.ID_ADAPTER AND a.DEVICE_ID = c.ID_DEVICE 
                     AND(
                       (a.DEVICE_TYPE_NAME = 'Elster A1800 (Электросчетчик)' AND c.PARAMETER_NAME = 'Канал 1')
                       OR((a.DEVICE_TYPE_NAME = 'СЭТ-4TM.03 (Электросчетчик)' OR a.DEVICE_TYPE_NAME = 'Меркурий 230ART (Электросчетчик)') AND c.PARAMETER_NAME = 'A+ (Энергия активная +)')
						OR (a.DEVICE_TYPE_NAME = 'СПГ761 (газовый корректор)' AND c.PARAMETER_NAME = 'Объем газа при ст. усл., тп1')
						OR (a.DEVICE_TYPE_NAME = 'СПТ961.1(.2) (тепловычислитель)' 
						AND c.PARAMETER_NAME =  'Тепловая энергия, тп1')					
						)
                      --список записей нужен либо для проверки отсутсвия записи, либо для вывода данных записи
					  left JOIN PA_RECORDS f on f.ID_ADAPTER = b.ID_ADAPTER and f.RECORD_TIME = t.RECORD_TIME and t.ID_DEVICE = f.ID_DEVICE and f.ID_RECORD>=@start_date and f.RECORD_TIME<=@max_date
					  
					  
					  --добавлем так же данные по записи
                    left JOIN PA_DATA d ON d.ID_RECORD = f.ID_RECORD AND d.ID_PARAMETER = c.ID_PARAMETER
         --ограничение по критерию, (максимальные и минимальные значения)
		
		 --место может не забито так что left join
		 left join DevicePlaces e on e.DEVICE_ID = a.DEVICE_ID and 
		--учитываем промежутки расположения приборов в указаных местах 
		e.BeginDateOfLocation<= t.RECORD_TIME and
		 e.EndDateOfLocation>= t.RECORD_TIME
		 and e.Multiplier>0
		 where
		 
		 (
		  	-- не пустой ID_Criteria означает что есть критерий по которому запись не подходит
	--				ID_Criteria is not NULL or 
					--пустой f.RECORD_TIME означает что записи нет
					f.RECORD_TIME is NULL ) 
					 --Если место пусто, значит прибор не стоял
			--		 and PlaceName is not NULL";

		public static string Creteria_filter = @"--ограничение по критерию, (максимальные и минимальные значения)
		 left join STG_CriteriaForDataFromDevices criteria on criteria.ID_DEVICE = a.DEVICE_ID and ( d.MEASURE_VALUE > criteria.Max_Param or d.MEASURE_VALUE < criteria.Min_Param or d.MEASURE_VALUE=0)";
		public static string ShowRecordsByStatus_all_recs_part1 = @"declare @start_date DateTime  
declare @cur_date DateTime

--дата по которой будем искать пропущенный данные
declare @max_date DateTime
set @start_date = CONVERT(Datetime, '";

		public static string ShowRecordsByStatus_all_recs_part2 = @"', 120)
set @max_date = CONVERT(Datetime, '";
		
		public static string ShowRecordsByStatus_all_recs_part3 = @"', 120)

SELECT ROW_NUMBER() OVER(ORDER BY  --e.PlaceName,
a.DEVICE_NAME, f.RECORD_TIME
ASC) AS 'ID', isnull(e.PlaceName,'Место не указано') as  PlaceName, 
                         a.DEVICE_NAME,a.DEVICE_ID,isnull(f.ID_RECORD, 0) as ID_RECORD, 
						--Если пропущенное время есть то выводим его, иначе время записи
						(f.RECORD_TIME


						) as RECORD_TIME,
						
						--Если данных по прибору нет, заменияем нолями
						ISNULL(d.PARAM_VALUE, 0) as PARAM_VALUE, ISNULL(d.MEASURE_VALUE, 0) as MEASURE_VALUE,
						
						 case
						 when ID_Criteria is not NULL then 'Не походит по критерию'
						-- when f.RECORD_TIME is NULL then 'Запись отсутствует'
				        when f.RECORD_TIME is not NULL  then 'Запись нормальная'


						  end as RecordStatus
					FROM PA_RECORDS f
					join PA_DEVICES a on f.ID_DEVICE = a.DEVICE_ID
					--ограничиваем по часовому или получасовму срезу в зависимости от итпа счетчика
					JOIN PA_ADAPTERS b ON b.ID_DEVICE = a.DEVICE_ID AND b.ADAPTER_LOGICAL_ID = 0 and f.ID_ADAPTER = b.ID_ADAPTER
					--ограничиваем по считываемому каналу
					JOIN PA_ADAPTER_PARAMETERS c ON c.ID_ADAPTER = b.ID_ADAPTER AND a.DEVICE_ID = c.ID_DEVICE

					 AND(
					   (a.DEVICE_TYPE_NAME = 'Elster A1800 (Электросчетчик)' AND c.PARAMETER_NAME = 'Канал 1')

					   OR((a.DEVICE_TYPE_NAME = 'СЭТ-4TM.03 (Электросчетчик)' OR a.DEVICE_TYPE_NAME = 'Меркурий 230ART (Электросчетчик)') AND c.PARAMETER_NAME = 'A+ (Энергия активная +)')
						OR (a.DEVICE_TYPE_NAME = 'СПГ761 (газовый корректор)' AND c.PARAMETER_NAME = 'Объем газа при ст. усл., тп1')
						OR (a.DEVICE_TYPE_NAME = 'СПТ961.1(.2) (тепловычислитель)' AND c.PARAMETER_NAME = 'Тепловая энергия, тп1')
					)
                      --список записей нужен либо для проверки отсутсвия записи, либо для вывода данных записи



					  --добавлем так же данные по записи

					left JOIN PA_DATA d ON d.ID_RECORD = f.ID_RECORD AND d.ID_PARAMETER = c.ID_PARAMETER"
			+Creteria_filter
		  + @"--место может не забито так что left join
		 left join DevicePlaces e on e.DEVICE_ID = a.DEVICE_ID and
		--учитываем промежутки расположения приборов в указаных местах
		e.BeginDateOfLocation <= f.RECORD_TIME and
		 e.EndDateOfLocation >= f.RECORD_TIME
and e.Multiplier>0

		 where
		 f.RECORD_TIME >= @start_date and
		  f.RECORD_TIME <= @max_date

--and
			--f.ID_ADAPTER = b.ID_ADAPTER
		 --  (
				--не пустой ID_Criteria означает что есть критерий по которому запись не подходит
  
					--  ID_Criteria is not NULL or
					  --пустой f.RECORD_TIME означает что записи нет
  
					 -- f.RECORD_TIME is NULL)
					   --Если место пусто, значит прибор не стоял
					-- and PlaceName is not NULL";

		public static string Csp_by_day_test(DateTime start_Date, DateTime end_Date, int placeId)
		{
			string sql_cmd= $@"DECLARE @csp table([Cycle] int
    ,[CycleDateEnd] Datetime
    ,[CycleDateBegin] Datetime
    ,[Place] nvarchar(max)
    ,[Weight] float
    ,[PlaceID] int
    ,[Composition] nvarchar(max),[day] int,[month] int
	,[year] int);
insert into @csp
select  * from OPENQUERY(GAMMA, N'


  SELECT Cycle, MAX(Date) AS [CycleDateEnd], MIN(Date) AS [CycleDateBegin], 
       --SpoolKind, 	  
			 Place, SUM(Weight) *1000 AS Weight, a.PlaceID, a.Composition,day,3  as month,2020 as year
FROM (
		SELECT ROW_NUMBER() OVER (ORDER BY d.PlaceID, d.Date) - RANK() 
							OVER (PARTITION BY d.PlaceID, nsp.SortValue + '' '' + nsp.Composition + '' '' + nsp.ColorGroup ORDER BY d.Date) AS [Cycle],DAY(d.Date) as day, d.Date, n.Name AS [SpoolKind], 
				p.Name AS [Place], dpp.Quantity AS [Weight], d.PlaceID,
				nsp.SortValue, 
				nsp.SortValue + '' '' + nsp.Composition + '' '' + nsp.ColorGroup AS Composition

				, nsp.RawMaterial
		FROM [GAMMA].[GammaNew].[dbo].[ProductSpools] ps 
		JOIN [GAMMA].[GammaNew].[dbo].[1CNomenclature] n ON n.[1CNomenclatureID] = ps.[1CNomenclatureID]
		JOIN [GAMMA].[GammaNew].[dbo].[1CCharacteristics] c ON ps.[1CCharacteristicID] = c.[1CCharacteristicID]
		JOIN [GAMMA].[GammaNew].[dbo].[DocProductionProducts] dpp ON dpp.ProductID = ps.ProductID
		JOIN [GAMMA].[GammaNew].[dbo].[Docs] d ON d.DocID = dpp.DocID
		JOIN [GAMMA].[GammaNew].[dbo].[Places] p ON p.PlaceID = d.PlaceID AND (d.PlaceID = {placeId})
		JOIN [GAMMA].[GammaNew].[dbo].[vNomenclatureSGBProperties] nsp ON nsp.[1CNomenclatureID] = n.[1CNomenclatureID]
		WHERE {StaticMethods.Filter_by_time(start_Date,end_Date,"d.Date",isDoubleQuote:true)} and d.PlaceID = {placeId}) a
GROUP BY Cycle, Place, PlaceID, a.SortValue,a.day,a.Composition, a.RawMaterial, a.SortValue

')";
			
			return sql_cmd;
		}
	}
}

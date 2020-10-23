using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class ScaffoldContext : DbContext
    {
        public ScaffoldContext()
        {
        }

        public ScaffoldContext(DbContextOptions<ScaffoldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DevicePlaces> DevicePlaces { get; set; }
        public virtual DbSet<PaActions> PaActions { get; set; }
        public virtual DbSet<PaAdapterParameters> PaAdapterParameters { get; set; }
        public virtual DbSet<PaAdapters> PaAdapters { get; set; }
        public virtual DbSet<PaComputers> PaComputers { get; set; }
        public virtual DbSet<PaData> PaData { get; set; }
        public virtual DbSet<PaDevices> PaDevices { get; set; }
        public virtual DbSet<PaJobs> PaJobs { get; set; }
        public virtual DbSet<PaLog> PaLog { get; set; }
        public virtual DbSet<PaParamMeasures> PaParamMeasures { get; set; }
        public virtual DbSet<PaRecords> PaRecords { get; set; }
        public virtual DbSet<PaSessions> PaSessions { get; set; }
        public virtual DbSet<PaUsers> PaUsers { get; set; }
        public virtual DbSet<PaVersions> PaVersions { get; set; }
        public virtual DbSet<StgBackgroundTasklogs> StgBackgroundTasklogs { get; set; }
        public virtual DbSet<StgCacheCycleSpoolProductionByDay> StgCacheCycleSpoolProductionByDay { get; set; }
        public virtual DbSet<StgCacheCycleSpoolProductionByMonth> StgCacheCycleSpoolProductionByMonth { get; set; }
        public virtual DbSet<StgCacheCycleSpoolProductionByShifts> StgCacheCycleSpoolProductionByShifts { get; set; }
        public virtual DbSet<StgCacheTerperMonths> StgCacheTerperMonths { get; set; }
        public virtual DbSet<StgConfigParams> StgConfigParams { get; set; }
        public virtual DbSet<StgCriteriaForDataFromDevices> StgCriteriaForDataFromDevices { get; set; }
        public virtual DbSet<StgDictionaryChangesPaRecords> StgDictionaryChangesPaRecords { get; set; }
        public virtual DbSet<StgEmailReportsNames> StgEmailReportsNames { get; set; }
        public virtual DbSet<StgEmailReportsReceivers> StgEmailReportsReceivers { get; set; }
        public virtual DbSet<StgLogTypes> StgLogTypes { get; set; }
        public virtual DbSet<StgPaRecordsChangesLog> StgPaRecordsChangesLog { get; set; }
        public virtual DbSet<StgPlacesNames> StgPlacesNames { get; set; }
        public virtual DbSet<StgPlanTer> StgPlanTer { get; set; }
        public virtual DbSet<StgReportsUsage> StgReportsUsage { get; set; }
        public virtual DbSet<StgSkuDictionary> StgSkuDictionary { get; set; }
        public virtual DbSet<StgSkuPlanTer> StgSkuPlanTer { get; set; }
        public virtual DbSet<StgSkuPlanTerTest> StgSkuPlanTerTest { get; set; }

        // Unable to generate entity type for table 'dbo.STG_Cache_CycleSpoolProduction'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.STG_Cache_TagDump'. Please see the warning messages.

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=PowerDB;Integrated Security=SSPI;Trusted_connection=True;MultipleActiveResultSets=true");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DevicePlaces>(entity =>
            {
                entity.HasKey(e => e.DevicePlaceId);

                entity.Property(e => e.DevicePlaceId)
                    .HasColumnName("DevicePlace_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BeginDateOfLocation)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('01.01.1900')");

                entity.Property(e => e.DeviceId).HasColumnName("DEVICE_ID");

                entity.Property(e => e.EndDateOfLocation)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('01.01.2099')");

                entity.Property(e => e.Multiplier).HasDefaultValueSql("((1))");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.PlaceName)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DevicePlaces)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_DevicePlaces_PA_DEVICES");
            });

            modelBuilder.Entity<PaActions>(entity =>
            {
                entity.HasKey(e => e.IdAction);

                entity.ToTable("PA_ACTIONS");

                entity.HasIndex(e => e.IdDevice)
                    .HasName("IX_PA_ACTIONS_1");

                entity.HasIndex(e => new { e.IdJob, e.ActionOrder })
                    .HasName("IX_PA_ACTIONS");

                entity.Property(e => e.IdAction).HasColumnName("ID_ACTION");

                entity.Property(e => e.ActionData)
                    .HasColumnName("ACTION_DATA")
                    .HasColumnType("image");

                entity.Property(e => e.ActionName)
                    .HasColumnName("ACTION_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('<Операция без названия>')");

                entity.Property(e => e.ActionOrder).HasColumnName("ACTION_ORDER");

                entity.Property(e => e.ActionTypeId).HasColumnName("ACTION_TYPE_ID");

                entity.Property(e => e.IdDevice).HasColumnName("ID_DEVICE");

                entity.Property(e => e.IdJob).HasColumnName("ID_JOB");

                entity.Property(e => e.IsEnabled)
                    .IsRequired()
                    .HasColumnName("IS_ENABLED")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(1)");

                entity.HasOne(d => d.IdDeviceNavigation)
                    .WithMany(p => p.PaActions)
                    .HasForeignKey(d => d.IdDevice)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PA_ACTIONS_PA_DEVICES");

                entity.HasOne(d => d.IdJobNavigation)
                    .WithMany(p => p.PaActions)
                    .HasForeignKey(d => d.IdJob)
                    .HasConstraintName("FK_PA_ACTIONS_PA_JOBS");
            });

            modelBuilder.Entity<PaAdapterParameters>(entity =>
            {
                entity.HasKey(e => e.IdParameter);

                entity.ToTable("PA_ADAPTER_PARAMETERS");

                entity.HasIndex(e => new { e.IdDevice, e.IdAdapter, e.ParameterOrder })
                    .HasName("IX_PA_ADAPTER_PARAMETERS");

                entity.Property(e => e.IdParameter).HasColumnName("ID_PARAMETER");

                entity.Property(e => e.IdAdapter).HasColumnName("ID_ADAPTER");

                entity.Property(e => e.IdDevice).HasColumnName("ID_DEVICE");

                entity.Property(e => e.IdMeasure)
                    .HasColumnName("ID_MEASURE")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.LogicalId)
                    .HasColumnName("LOGICAL_ID")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.MulKoeff)
                    .HasColumnName("MUL_KOEFF")
                    .HasDefaultValueSql("(1)");

                entity.Property(e => e.OverloadValue)
                    .HasColumnName("OVERLOAD_VALUE")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.ParamType)
                    .HasColumnName("PARAM_TYPE")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.ParameterName)
                    .IsRequired()
                    .HasColumnName("PARAMETER_NAME")
                    .HasMaxLength(99)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('<Параметр без названия>')");

                entity.Property(e => e.ParameterOrder)
                    .HasColumnName("PARAMETER_ORDER")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.StatusParam)
                    .HasColumnName("STATUS_PARAM")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.ValueType)
                    .HasColumnName("VALUE_TYPE")
                    .HasDefaultValueSql("(0)");

                entity.HasOne(d => d.Id)
                    .WithMany(p => p.PaAdapterParameters)
                    .HasPrincipalKey(p => new { p.IdDevice, p.IdAdapter })
                    .HasForeignKey(d => new { d.IdDevice, d.IdAdapter })
                    .HasConstraintName("FK_PA_ADAPTER_PARAMETERS_PA_ADAPTERS");
            });

            modelBuilder.Entity<PaAdapters>(entity =>
            {
                entity.HasKey(e => e.IdAdapter);

                entity.ToTable("PA_ADAPTERS");

                entity.HasIndex(e => new { e.IdDevice, e.IdAdapter })
                    .HasName("IX_PA_ADAPTERS")
                    .IsUnique();

                entity.Property(e => e.IdAdapter).HasColumnName("ID_ADAPTER");

                entity.Property(e => e.AdapterDescription)
                    .HasColumnName("ADAPTER_DESCRIPTION")
                    .HasColumnType("text");

                entity.Property(e => e.AdapterLogicalId)
                    .HasColumnName("ADAPTER_LOGICAL_ID")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.AdapterName)
                    .HasColumnName("ADAPTER_NAME")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Адаптер')");

                entity.Property(e => e.AdapterOrder)
                    .HasColumnName("ADAPTER_ORDER")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.AdapterTypeId)
                    .HasColumnName("ADAPTER_TYPE_ID")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.AdapterTypeName)
                    .HasColumnName("ADAPTER_TYPE_NAME")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Адаптер')");

                entity.Property(e => e.IdDevice).HasColumnName("ID_DEVICE");

                entity.HasOne(d => d.IdDeviceNavigation)
                    .WithMany(p => p.PaAdapters)
                    .HasForeignKey(d => d.IdDevice)
                    .HasConstraintName("FK_PA_ADAPTERS_PA_DEVICES");
            });

            modelBuilder.Entity<PaComputers>(entity =>
            {
                entity.HasKey(e => e.IdComputer);

                entity.ToTable("PA_COMPUTERS");

                entity.Property(e => e.IdComputer).HasColumnName("ID_COMPUTER");

                entity.Property(e => e.DescComputer)
                    .HasColumnName("DESC_COMPUTER")
                    .HasColumnType("text");

                entity.Property(e => e.NameComputer)
                    .IsRequired()
                    .HasColumnName("NAME_COMPUTER")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaData>(entity =>
            {
                entity.HasKey(e => new { e.IdRecord, e.IdParameter });

                entity.ToTable("PA_DATA");

                entity.Property(e => e.IdRecord).HasColumnName("ID_RECORD");

                entity.Property(e => e.IdParameter).HasColumnName("ID_PARAMETER");

                entity.Property(e => e.MeasureValue).HasColumnName("MEASURE_VALUE");

                entity.Property(e => e.ParamValue).HasColumnName("PARAM_VALUE");

                entity.HasOne(d => d.IdRecordNavigation)
                    .WithMany(p => p.PaData)
                    .HasForeignKey(d => d.IdRecord)
                    .HasConstraintName("FK_PA_DATA_PA_RECORDS");
            });

            modelBuilder.Entity<PaDevices>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.ToTable("PA_DEVICES");

                entity.Property(e => e.DeviceId).HasColumnName("DEVICE_ID");

                entity.Property(e => e.DeviceClsid)
                    .IsRequired()
                    .HasColumnName("DEVICE_CLSID")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceData)
                    .HasColumnName("DEVICE_DATA")
                    .HasColumnType("image");

                entity.Property(e => e.DeviceDescription)
                    .HasColumnName("DEVICE_DESCRIPTION")
                    .HasColumnType("text");

                entity.Property(e => e.DeviceName)
                    .HasColumnName("DEVICE_NAME")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceType).HasColumnName("DEVICE_TYPE");

                entity.Property(e => e.DeviceTypeName)
                    .IsRequired()
                    .HasColumnName("DEVICE_TYPE_NAME")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PaJobs>(entity =>
            {
                entity.HasKey(e => e.IdJob);

                entity.ToTable("PA_JOBS");

                entity.Property(e => e.IdJob).HasColumnName("ID_JOB");

                entity.Property(e => e.IsEnabled)
                    .IsRequired()
                    .HasColumnName("IS_ENABLED")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(1)");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnName("JOB_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('(Название не задано)')");

                entity.Property(e => e.JobOption)
                    .HasColumnName("JOB_OPTION")
                    .HasColumnType("image");

                entity.Property(e => e.MaxExecTime)
                    .HasColumnName("MAX_EXEC_TIME")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('0:00:00')");

                entity.Property(e => e.OffsetTime)
                    .HasColumnName("OFFSET_TIME")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('0:00:00')");

                entity.Property(e => e.StartMode)
                    .HasColumnName("START_MODE")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.StartTime)
                    .HasColumnName("START_TIME")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('0:00:00')");
            });

            modelBuilder.Entity<PaLog>(entity =>
            {
                entity.HasKey(e => e.IdLog);

                entity.ToTable("PA_LOG");

                entity.HasIndex(e => e.Status)
                    .HasName("IX_PA_LOG_2");

                entity.HasIndex(e => new { e.IdDevice, e.DateTime })
                    .HasName("IX_PA_LOG_1");

                entity.HasIndex(e => new { e.IdSession, e.DateTime })
                    .HasName("IX_PA_LOG");

                entity.Property(e => e.IdLog).HasColumnName("ID_LOG");

                entity.Property(e => e.DateTime)
                    .HasColumnName("DATE_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdDevice).HasColumnName("ID_DEVICE");

                entity.Property(e => e.IdSession).HasColumnName("ID_SESSION");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.Message)
                    .HasColumnName("MESSAGE")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.HasOne(d => d.IdDeviceNavigation)
                    .WithMany(p => p.PaLog)
                    .HasForeignKey(d => d.IdDevice)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PA_LOG_PA_DEVICES");

                entity.HasOne(d => d.IdSessionNavigation)
                    .WithMany(p => p.PaLog)
                    .HasForeignKey(d => d.IdSession)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PA_LOG_PA_SESSIONS");
            });

            modelBuilder.Entity<PaParamMeasures>(entity =>
            {
                entity.HasKey(e => new { e.IdParameter, e.IdMeasure });

                entity.ToTable("PA_PARAM_MEASURES");

                entity.HasIndex(e => e.IdParameter)
                    .HasName("IX_PA_PARAM_MEASURES");

                entity.Property(e => e.IdParameter).HasColumnName("ID_PARAMETER");

                entity.Property(e => e.IdMeasure).HasColumnName("ID_MEASURE");

                entity.Property(e => e.Identifier)
                    .HasColumnName("IDENTIFIER")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsDefault)
                    .IsRequired()
                    .HasColumnName("IS_DEFAULT")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.IsMultiplier)
                    .IsRequired()
                    .HasColumnName("IS_MULTIPLIER")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('1')");

                entity.Property(e => e.Koeff)
                    .HasColumnName("KOEFF")
                    .HasDefaultValueSql("(1)");

                entity.Property(e => e.MeasureName)
                    .IsRequired()
                    .HasColumnName("MEASURE_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('<Название не задано>')");

                entity.HasOne(d => d.IdParameterNavigation)
                    .WithMany(p => p.PaParamMeasures)
                    .HasForeignKey(d => d.IdParameter)
                    .HasConstraintName("FK_PA_PARAM_MEASURES_PA_ADAPTER_PARAMETERS");
            });

            modelBuilder.Entity<PaRecords>(entity =>
            {
                entity.HasKey(e => e.IdRecord);

                entity.ToTable("PA_RECORDS");

                entity.HasIndex(e => new { e.IdDevice, e.IdAdapter, e.MethodType, e.RecordTime, e.RecordIndex })
                    .HasName("IX_PA_RECORDS")
                    .IsUnique();

                entity.Property(e => e.IdRecord).HasColumnName("ID_RECORD");

                entity.Property(e => e.IdAdapter).HasColumnName("ID_ADAPTER");

                entity.Property(e => e.IdDevice).HasColumnName("ID_DEVICE");

                entity.Property(e => e.MethodType)
                    .HasColumnName("METHOD_TYPE")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.RecordIndex).HasColumnName("RECORD_INDEX");

                entity.Property(e => e.RecordTime)
                    .HasColumnName("RECORD_TIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("(0)");

                entity.HasOne(d => d.Id)
                    .WithMany(p => p.PaRecords)
                    .HasPrincipalKey(p => new { p.IdDevice, p.IdAdapter })
                    .HasForeignKey(d => new { d.IdDevice, d.IdAdapter })
                    .HasConstraintName("FK_PA_RECORDS_PA_ADAPTERS");
            });

            modelBuilder.Entity<PaSessions>(entity =>
            {
                entity.HasKey(e => e.IdSession);

                entity.ToTable("PA_SESSIONS");

                entity.HasIndex(e => e.ActiveUpTo)
                    .HasName("IX_PA_SESSIONS_3");

                entity.HasIndex(e => e.IdComputer)
                    .HasName("IX_PA_SESSIONS");

                entity.HasIndex(e => e.PossiblyActive)
                    .HasName("IX_PA_SESSIONS_2");

                entity.HasIndex(e => e.StartSession)
                    .HasName("IX_PA_SESSIONS_1");

                entity.Property(e => e.IdSession).HasColumnName("ID_SESSION");

                entity.Property(e => e.ActiveUpTo)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.FinishSession)
                    .HasColumnName("FINISH_SESSION")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdComputer).HasColumnName("ID_COMPUTER");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.PossiblyActive).HasDefaultValueSql("(0)");

                entity.Property(e => e.ProgTitle)
                    .HasColumnName("PROG_TITLE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StartSession)
                    .HasColumnName("START_SESSION")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdComputerNavigation)
                    .WithMany(p => p.PaSessions)
                    .HasForeignKey(d => d.IdComputer)
                    .HasConstraintName("FK_PA_SESSIONS_PA_COMPUTERS");
            });

            modelBuilder.Entity<PaUsers>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("PA_USERS");

                entity.HasIndex(e => e.UserLogin)
                    .HasName("IX_PA_USERS")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.UserDescription)
                    .HasColumnName("USER_DESCRIPTION")
                    .HasColumnType("text");

                entity.Property(e => e.UserLogin)
                    .IsRequired()
                    .HasColumnName("USER_LOGIN")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasColumnName("USER_PASSWORD")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole).HasColumnName("USER_ROLE");
            });

            modelBuilder.Entity<PaVersions>(entity =>
            {
                entity.HasKey(e => e.IdVersion);

                entity.ToTable("PA_VERSIONS");

                entity.Property(e => e.IdVersion).HasColumnName("ID_VERSION");

                entity.Property(e => e.Changes)
                    .HasColumnName("CHANGES")
                    .HasColumnType("text");

                entity.Property(e => e.Ver).HasColumnName("VER");

                entity.Property(e => e.VerDate)
                    .HasColumnName("VER_DATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<StgBackgroundTasklogs>(entity =>
            {
                entity.ToTable("STG_BackgroundTasklogs");

                entity.Property(e => e.LogDateTime).HasColumnType("datetime");

                entity.Property(e => e.TimerName).IsRequired();

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.StgBackgroundTasklogs)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STG_BackgroundTasklogs_STG_Log_types");
            });

            modelBuilder.Entity<StgCacheCycleSpoolProductionByDay>(entity =>
            {
                entity.ToTable("STG_Cache_CycleSpoolProduction_byDay");

                entity.Property(e => e.AverageSpeed).HasDefaultValueSql("((0))");

                entity.Property(e => e.Composition).IsUnicode(false);

                entity.Property(e => e.CycleDateBegin).HasColumnType("datetime");

                entity.Property(e => e.CycleDateEnd).HasColumnType("datetime");

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.Place).IsUnicode(false);

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<StgCacheCycleSpoolProductionByMonth>(entity =>
            {
                entity.ToTable("STG_Cache_CycleSpoolProduction_byMonth");

                entity.Property(e => e.AverageSpeed).HasDefaultValueSql("((0))");

                entity.Property(e => e.Composition).IsUnicode(false);

                entity.Property(e => e.CycleDateBegin).HasColumnType("datetime");

                entity.Property(e => e.CycleDateEnd).HasColumnType("datetime");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.Place).IsUnicode(false);

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<StgCacheCycleSpoolProductionByShifts>(entity =>
            {
                entity.ToTable("STG_Cache_CycleSpoolProduction_byShifts");

                entity.Property(e => e.AverageSpeed).HasDefaultValueSql("((0))");

                entity.Property(e => e.Composition).IsUnicode(false);

                entity.Property(e => e.CycleDateBegin).HasColumnType("datetime");

                entity.Property(e => e.CycleDateEnd).HasColumnType("datetime");

                entity.Property(e => e.MachinistName).HasColumnName("Machinist_name");

                entity.Property(e => e.Place).IsUnicode(false);

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            });

            modelBuilder.Entity<StgCacheTerperMonths>(entity =>
            {
                entity.ToTable("STG_Cache_TERperMonths");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.PlaceName).IsRequired();

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnName("SKU");

                entity.Property(e => e.ValueOfBo).HasColumnName("ValueOfBO");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<StgConfigParams>(entity =>
            {
                entity.ToTable("STG_ConfigParams");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ParamName).IsRequired();

                entity.Property(e => e.ParamVal).IsRequired();
            });

            modelBuilder.Entity<StgCriteriaForDataFromDevices>(entity =>
            {
                entity.HasKey(e => e.IdCriteria);

                entity.ToTable("STG_CriteriaForDataFromDevices");

                entity.Property(e => e.IdCriteria)
                    .HasColumnName("ID_Criteria")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdDevice).HasColumnName("ID_DEVICE");

                entity.Property(e => e.MaxParam)
                    .HasColumnName("Max_Param")
                    .HasDefaultValueSql("((1000000))");

                entity.Property(e => e.MinParam).HasColumnName("Min_Param");

                entity.HasOne(d => d.IdDeviceNavigation)
                    .WithMany(p => p.StgCriteriaForDataFromDevices)
                    .HasForeignKey(d => d.IdDevice)
                    .HasConstraintName("FK_STG_CriteriaForDataFromDevices_PA_DEVICES");
            });

            modelBuilder.Entity<StgDictionaryChangesPaRecords>(entity =>
            {
                entity.HasKey(e => e.IdDictionaryChangesPaRecords);

                entity.ToTable("STG_Dictionary_changes_PaRecords");

                entity.Property(e => e.IdDictionaryChangesPaRecords)
                    .HasColumnName("Id_Dictionary_changes_PaRecords")
                    .ValueGeneratedNever();

                entity.Property(e => e.NameOfChange)
                    .IsRequired()
                    .HasColumnName("Name_Of_Change")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<StgEmailReportsNames>(entity =>
            {
                entity.ToTable("STG_Email_Reports_Names");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DelayTime).HasColumnName("Delay_time");

                entity.Property(e => e.ReportName)
                    .IsRequired()
                    .HasColumnName("Report_name");

                entity.Property(e => e.ReportNameEng).HasColumnName("Report_name_eng");
            });

            modelBuilder.Entity<StgEmailReportsReceivers>(entity =>
            {
                entity.HasKey(e => e.EmailReportsReceiversId);

                entity.ToTable("STG_Email_Reports_Receivers");

                entity.Property(e => e.EmailReportsReceiversId).HasColumnName("Email_Reports_Receivers_id");

                entity.Property(e => e.EmailName)
                    .IsRequired()
                    .HasColumnName("email_name")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportNotificationNameId).HasColumnName("Report_notification_name_id");

                entity.HasOne(d => d.ReportNotificationName)
                    .WithMany(p => p.StgEmailReportsReceivers)
                    .HasForeignKey(d => d.ReportNotificationNameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STG_Email_Reports_Receivers");
            });

            modelBuilder.Entity<StgLogTypes>(entity =>
            {
                entity.ToTable("STG_Log_types");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Typename)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<StgPaRecordsChangesLog>(entity =>
            {
                entity.HasKey(e => e.IdPaRecordsChangesLog);

                entity.ToTable("STG_PaRecords_changes_log");

                entity.Property(e => e.IdPaRecordsChangesLog).HasColumnName("ID_PaRecords_changes_log");

                entity.Property(e => e.DateChange)
                    .HasColumnName("date_change")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdChangesStatus).HasColumnName("ID_ChangesStatus");

                entity.Property(e => e.IdRecord).HasColumnName("ID_Record");

                entity.Property(e => e.NewMeasureValue).HasColumnName("new_measure_value");

                entity.Property(e => e.NewParamValue).HasColumnName("new_param_value");

                entity.Property(e => e.OldMeasureValue).HasColumnName("old_measure_value");

                entity.Property(e => e.OldParamValue).HasColumnName("old_param_value");

                entity.HasOne(d => d.IdRecordNavigation)
                    .WithMany(p => p.StgPaRecordsChangesLog)
                    .HasForeignKey(d => d.IdRecord)
                    .HasConstraintName("FK_PaRecords_changes_log_ToTable");
            });

            modelBuilder.Entity<StgPlacesNames>(entity =>
            {
                entity.HasKey(e => e.PlacesNamesId);

                entity.ToTable("STG_PlacesNames");

                entity.Property(e => e.PlacesNamesId).HasColumnName("PlacesNamesID");

                entity.Property(e => e.PlaceName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StgPlanTer>(entity =>
            {
                entity.HasKey(e => e.IdStgPlanTer);

                entity.ToTable("STG_Plan_TER");

                entity.HasIndex(e => new { e.Sku, e.PlaceId, e.Year, e.Month })
                    .HasName("NonClusteredIndex-20200327-120420");

                entity.Property(e => e.IdStgPlanTer).HasColumnName("Id_STG_Plan_TER");

                entity.Property(e => e.AvarageSpeedMin).HasColumnName("Avarage_speed_min");

                entity.Property(e => e.AvarageSpeedNorm).HasColumnName("Avarage_speed_norm");

                entity.Property(e => e.BoValue).HasColumnName("BO_Value");

                entity.Property(e => e.EnergyPerTonneMin).HasColumnName("Energy_perTonne_min");

                entity.Property(e => e.EnergyPerTonneNorm).HasColumnName("Energy_perTonne_norm");

                entity.Property(e => e.GasPerTonneMin).HasColumnName("Gas_perTonne_min");

                entity.Property(e => e.GasPerTonneNorm).HasColumnName("Gas_perTonne_norm");

                entity.Property(e => e.ShiftProductivityMin).HasColumnName("Shift_productivity_min");

                entity.Property(e => e.ShiftProductivityNorm).HasColumnName("Shift_productivity_norm");

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                entity.Property(e => e.SteamPerTonneMin).HasColumnName("Steam_perTonne_min");

                entity.Property(e => e.SteamPerTonneNorm).HasColumnName("Steam_perTonne_norm");
            });

            modelBuilder.Entity<StgReportsUsage>(entity =>
            {
                entity.ToTable("STG_Reports_Usage");

                entity.Property(e => e.AccessDate).HasColumnType("datetime");

                entity.Property(e => e.RemoteIp).HasMaxLength(15);
            });

            modelBuilder.Entity<StgSkuDictionary>(entity =>
            {
                entity.HasKey(e => e.IdSkuDictionary);

                entity.ToTable("STG_SKU_Dictionary");

                entity.Property(e => e.IdSkuDictionary).HasColumnName("Id_SKU_Dictionary");

                entity.Property(e => e.SkuName)
                    .IsRequired()
                    .HasColumnName("SKU_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.SkuNameFullName).HasColumnName("SKU_Name_FullName");
            });

            modelBuilder.Entity<StgSkuPlanTer>(entity =>
            {
                entity.HasKey(e => e.IdSkuPlanTer);

                entity.ToTable("STG_SKU_Plan_TER");

                entity.Property(e => e.IdSkuPlanTer).HasColumnName("Id_SKU_Plan_TER");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.PlaceName).IsRequired();

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnName("SKU");

                entity.Property(e => e.ValueOfBo).HasColumnName("ValueOfBO");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<StgSkuPlanTerTest>(entity =>
            {
                entity.HasKey(e => e.IdSkuPlanTer);

                entity.ToTable("STG_SKU_Plan_TER_test");

                entity.Property(e => e.IdSkuPlanTer).HasColumnName("Id_SKU_Plan_TER");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.PlaceName).IsRequired();

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnName("SKU");

                entity.Property(e => e.ValueOfBo).HasColumnName("ValueOfBO");

                entity.Property(e => e.Year).HasColumnName("year");
            });
        }
    }
}

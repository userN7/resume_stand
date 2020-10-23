using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG_counters_admin_interface.Tests
{
	public class TestDataGenerator_VerifyModelData : IEnumerable<object[]>
	{
#pragma warning disable CS0414// these fields used for information only
		private readonly string creationDate = "21.10.2020 19:08:45";
		private readonly string startDate = "01.06.2020 0:00:00";
		private readonly string endDate = "01.06.2020 23:59:59";
#pragma warning restore CS0414
		private readonly List<object[]> _data = new List<object[]>
	{
 new object[]  {"ConsumptionByCycleByBDM1;БДМ-1 по видам продукции",
@"CycleDateBegin=01.06.2020 0:00:00
CycleDateEnd=03.06.2020 10:04:23
Place=БДМ-1
SortofProduction=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=145,998
CapacityOfBO=147,55
EnergyConsumptionPerTonne=0,989
GasConsumption=22,88
SteamConsumption=114,9
GasConsumptionPerTonne=0,155
SteamConsumptionPerTonne=0,779
AverageSpeed=1393
"
},
 new object[]  {"EnergyConsumptionByManufactureByHour;Часовой расход электроэнергии по производству",
@"Date=01.06.2020
Time=0:00
2739,401
42,075
2781,476
1937,09
0,8
75,23
76,03
4794,596
29,498
34,932
67,308
19,666
151,404
4946
20,56
166,2
7,275
2,175
18,675
5160,885
Date=01.06.2020
Time=1:00
2673,326
42,3
2715,626
1908,8
0,8
83,77
84,57
4708,996
31,81
39,288
64,5
21,206
156,804
4865,8
20,56
163
6,925
2,15
19,125
5077,56
Date=01.06.2020
Time=2:00
2728,002
42,3
2770,302
1961,235
0,82
50,6
51,42
4782,957
30,773
37,38
48,42
20,515
137,088
4920,045
20,76
165,32
6,8
2,15
20,925
5136
Date=01.06.2020
Time=3:00
2574,253
40,275
2614,528
1947,685
0,82
49,1
49,92
4612,133
29,549
37,392
69,912
19,699
156,552
4768,685
20,68
163,76
6,825
2,05
20,475
4982,475
Date=01.06.2020
Time=4:00
2650,632
36,9
2687,532
1919,63
0,8
86,58
87,38
4694,542
32,306
41,328
56,916
21,538
152,088
4846,63
20,72
165,24
6,7
2,225
20,025
5061,54
Date=01.06.2020
Time=5:00
2669,108
36,9
2706,008
1944,13
0,82
79,98
80,8
4730,938
30,665
43,116
34,968
20,443
129,192
4860,13
20,76
157,24
6,85
2,1
20,25
5067,33
Date=01.06.2020
Time=6:00
2634,171
36,45
2670,621
1973,265
0,82
60,62
61,44
4705,326
18,641
34,356
30,96
12,427
96,384
4801,71
20,72
150,72
6,925
2,1
19,125
5001,3
Date=01.06.2020
Time=7:00
2556,552
36,45
2593,002
1954,07
0,82
69,69
70,51
4617,582
14,76
29,532
17,676
9,84
71,808
4689,39
20,72
150,56
8,625
2,525
18,675
4890,495
Date=01.06.2020
Time=8:00
2630,571
36,9
2667,471
1938
0,82
110,06
110,88
4716,351
10,116
28,236
22,848
6,744
67,944
4784,295
20,72
150,64
14,525
2,725
21,375
4994,28
Date=01.06.2020
Time=9:00
2663,42
36,9
2700,32
1953,07
0,8
78,42
79,22
4732,61
29,002
34,5
69,264
19,334
152,1
4884,71
20,6
168,4
16
2,325
25,2
5117,235
Date=01.06.2020
Time=10:00
2629,779
36,675
2666,454
1942,755
0,84
121,3
122,14
4731,349
31,55
43,896
66,576
21,034
163,056
4894,405
20,64
169,2
15,3
2,6
24,75
5126,895
Date=01.06.2020
Time=11:00
2656,92
36,9
2693,82
1978,245
0,82
88,24
89,06
4761,125
26,489
39,168
69,624
17,659
152,94
4914,065
20,6
173,44
15,425
2,6
24,075
5150,205
Date=01.06.2020
Time=12:00
2717,707
36,45
2754,157
1966,895
0,8
100,31
101,11
4822,162
30,247
41,952
64,884
20,165
157,248
4979,41
20,56
170,43
16,175
2,3
22,95
5211,825
Date=01.06.2020
Time=13:00
2654,075
37,35
2691,425
1970,905
0,82
79,19
80,01
4742,34
26,258
40,272
70,764
17,506
154,8
4897,14
20,6
165,41
15,45
2,525
19,575
5120,7
Date=01.06.2020
Time=14:00
2710,863
36,9
2747,763
1955,37
0,8
104,7
105,5
4808,633
30,334
42,984
71,952
20,222
165,492
4974,125
20,6
165,89
14,2
2,525
20,25
5197,59
Date=01.06.2020
Time=15:00
2656,769
36,675
2693,444
1960,78
0,8
80,24
81,04
4735,264
30,182
43,32
18,792
20,122
112,416
4847,68
20,6
154,34
14,3
2
19,8
5058,72
Date=01.06.2020
Time=16:00
2691,403
36,675
2728,078
1944,985
0,82
103,98
104,8
4777,863
28,663
39,852
16,968
19,109
104,592
4882,455
20,56
155,29
13,775
2,225
19,35
5093,655
Date=01.06.2020
Time=17:00
2652,673
36,45
2689,123
1956,745
0,8
91,93
92,73
4738,598
29,686
40,92
22,116
19,79
112,512
4851,11
20,6
158,08
11,9
2,375
19,125
5063,19
Date=01.06.2020
Time=18:00
2664,702
36,9
2701,602
1964,385
0,82
78,12
78,94
4744,927
28,159
36,144
16,692
18,773
99,768
4844,695
20,64
151,36
8,275
1,95
18,45
5045,37
Date=01.06.2020
Time=19:00
2667,219
36,45
2703,669
1972,79
0,8
64,74
65,54
4741,999
14,184
16,14
20,076
9,456
59,856
4801,855
20,6
148,01
8,2
2,25
18,9
4999,815
Date=01.06.2020
Time=20:00
2637,596
36,675
2674,271
1951,705
0,8
71,84
72,64
4698,616
13,406
28,176
37,464
8,938
87,984
4786,6
20,68
160,87
8,475
2,25
20,475
4999,35
Date=01.06.2020
Time=21:00
2628,338
37,35
2665,688
1951,275
0,82
71,97
72,79
4689,753
22,414
39,792
36,504
14,942
113,652
4803,405
20,76
157,41
7,175
1,975
22,05
5012,775
Date=01.06.2020
Time=22:00
2631,741
42,075
2673,816
1971,485
0,82
64,39
65,21
4710,511
28,922
42,264
44,076
19,282
134,544
4845,055
20,76
165,69
7,2
2,225
21,15
5062,08
Date=01.06.2020
Time=23:00
2632,823
41,85
2674,673
2070,735
0,82
79,71
80,53
4825,938
32,443
37,356
54,504
21,629
145,932
4971,87
20,8
163,91
6,925
2,225
20,475
5186,205
"
},
 new object[]  {"TERperMonthperTonne;Средневзвешенное ТЭР на тонну по циклам производства",
@"PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=1
ValueOfBO=612,491
AverageSpeed=1379
EnegryConsumptionperTonne=0,94
SteamConsumptionperTonne=0,678
GasConsumptionperTonne=0,146
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=1
ValueOfBO=334,85
AverageSpeed=1531
EnegryConsumptionperTonne=0,675
SteamConsumptionperTonne=1,006
GasConsumptionperTonne=0,156
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=2
ValueOfBO=695,24
AverageSpeed=1361
EnegryConsumptionperTonne=0,958
SteamConsumptionperTonne=0,712
GasConsumptionperTonne=0,144
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=2
ValueOfBO=737,08
AverageSpeed=1595
EnegryConsumptionperTonne=0,664
SteamConsumptionperTonne=0,995
GasConsumptionperTonne=0,157
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=3
ValueOfBO=480,31
AverageSpeed=1281
EnegryConsumptionperTonne=0,968
SteamConsumptionperTonne=0,743
GasConsumptionperTonne=0,147
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=3
ValueOfBO=431,17
AverageSpeed=1612
EnegryConsumptionperTonne=0,71
SteamConsumptionperTonne=0,978
GasConsumptionperTonne=0,159
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=4
ValueOfBO=534,78
AverageSpeed=1223
EnegryConsumptionperTonne=1,014
SteamConsumptionperTonne=0,694
GasConsumptionperTonne=0,149
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=4
ValueOfBO=614,96
AverageSpeed=1618
EnegryConsumptionperTonne=0,693
SteamConsumptionperTonne=0,993
GasConsumptionperTonne=0,161
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=5
ValueOfBO=1308,71
AverageSpeed=1279
EnegryConsumptionperTonne=1,004
SteamConsumptionperTonne=0,753
GasConsumptionperTonne=0,156
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=5
ValueOfBO=489,68
AverageSpeed=1544
EnegryConsumptionperTonne=0,649
SteamConsumptionperTonne=1,068
GasConsumptionperTonne=0,172
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=6
ValueOfBO=879,95
AverageSpeed=1405
EnegryConsumptionperTonne=0,99
SteamConsumptionperTonne=0,707
GasConsumptionperTonne=0,147
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=6
ValueOfBO=575,61
AverageSpeed=1569
EnegryConsumptionperTonne=0,674
SteamConsumptionperTonne=1,041
GasConsumptionperTonne=0,168
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=7
ValueOfBO=558,56
AverageSpeed=1220
EnegryConsumptionperTonne=1,027
SteamConsumptionperTonne=0,861
GasConsumptionperTonne=0,159
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=7
ValueOfBO=755,39
AverageSpeed=1579
EnegryConsumptionperTonne=0,703
SteamConsumptionperTonne=1,049
GasConsumptionperTonne=0,138
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=8
ValueOfBO=778,19
AverageSpeed=1385
EnegryConsumptionperTonne=0,943
SteamConsumptionperTonne=0,761
GasConsumptionperTonne=0,123
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=8
ValueOfBO=610,44
AverageSpeed=1553
EnegryConsumptionperTonne=0,772
SteamConsumptionperTonne=0,96
GasConsumptionperTonne=0,128
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=9
ValueOfBO=867,19
AverageSpeed=1357
EnegryConsumptionperTonne=0,928
SteamConsumptionperTonne=0,833
GasConsumptionperTonne=0,153
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=9
ValueOfBO=667,31
AverageSpeed=1566
EnegryConsumptionperTonne=0,698
SteamConsumptionperTonne=1,102
GasConsumptionperTonne=0,132
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=10
ValueOfBO=924,99
AverageSpeed=1350
EnegryConsumptionperTonne=0,888
SteamConsumptionperTonne=0,874
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=10
ValueOfBO=170,34
AverageSpeed=1622
EnegryConsumptionperTonne=0,639
SteamConsumptionperTonne=1,056
GasConsumptionperTonne=0,134
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=13
ValueOfBO=7640,411
AverageSpeed=1323
EnegryConsumptionperTonne=0,964
SteamConsumptionperTonne=0,766
GasConsumptionperTonne=0,148
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=13
ValueOfBO=5386,83
AverageSpeed=1578
EnegryConsumptionperTonne=0,692
SteamConsumptionperTonne=1,024
GasConsumptionperTonne=0,15
PlaceID=1
PlaceName=БДМ-1
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=6
CycleDateBegin=01.06.2020 0:00:00
CycleDateEnd=03.06.2020 10:04:23
ValueOfBO=147,55
AverageSpeed=1393
EnegryConsumptionperTonne=0,989
SteamConsumptionperTonne=0,779
GasConsumptionperTonne=0,155
PlaceID=2
PlaceName=БДМ-2
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=6
CycleDateBegin=01.06.2020 0:00:00
CycleDateEnd=02.06.2020 1:59:19
ValueOfBO=77,44
AverageSpeed=1649
EnegryConsumptionperTonne=0,662
SteamConsumptionperTonne=1,094
GasConsumptionperTonne=0,173
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=1
ValueOfBO=204
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=2
ValueOfBO=593
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=3
ValueOfBO=837
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=4
ValueOfBO=822
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=5
ValueOfBO=0
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=6
ValueOfBO=0
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=7
ValueOfBO=0
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=8
ValueOfBO=0
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=9
ValueOfBO=0
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=1
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=10
ValueOfBO=0
AverageSpeed=1400
EnegryConsumptionperTonne=1,013
SteamConsumptionperTonne=0,724
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=1
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=2
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=3
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=4
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=5
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=6
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=7
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=8
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=9
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
PlaceID=2
PlaceName=
SKU=БОТ Ц-84 код A БЕЛЫЙ
Month=10
ValueOfBO=0
AverageSpeed=1590
EnegryConsumptionperTonne=0,695
SteamConsumptionperTonne=0,942
GasConsumptionperTonne=0,151
"
},
 new object[]  {"ConsumptionByBDM1ByDay;БДМ-1(Суточный)",
@"CycleDateBegin=01.06.2020 0:00:00
CycleDateEnd=01.06.2020 23:59:59
Place=БДМ-1
SortofProduction=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=64,665
CapacityOfBO=65,58
EnergyConsumptionPerTonne=0,986
GasConsumption=9,696
SteamConsumption=47,87
GasConsumptionPerTonne=0,148
SteamConsumptionPerTonne=0,73
AverageSpeed=1469
"
},
 new object[]  {"ConsumptionByBDM2ByDay;БДМ-2(Суточный)",
@"CycleDateBegin=01.06.2020 0:00:00
CycleDateEnd=01.06.2020 23:59:59
Place=БДМ-2
SortofProduction=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=46,996
CapacityOfBO=69,51
EnergyConsumptionPerTonne=0,676
GasConsumption=12,388
SteamConsumption=78,25
GasConsumptionPerTonne=0,178
SteamConsumptionPerTonne=1,126
AverageSpeed=1653
"
},
 new object[]  {"EnergyConsumptionByDevicesByDay;Суточный расход электроэнергии по учётам",
@"Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=36253,575
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=579,375
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=19751,175
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=8810,775
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=9413,775
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=6808,05
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=8061,975
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=333,45
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=8770,5
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=495,225
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=13610,7
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=1093,764
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=1050,096
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=892,296
Measure_Date=01.06.2020 0:00:00
Device_Name=АБК
EnergyConsumption=250,225
Measure_Date=01.06.2020 0:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=3203,6
Measure_Date=01.06.2020 0:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=Компрессор
EnergyConsumption=955,53
Measure_Date=01.06.2020 0:00:00
Device_Name=ПРС-2
EnergyConsumption=1347,96
Measure_Date=01.06.2020 0:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=596,75
Measure_Date=01.06.2020 0:00:00
Device_Name=Столовая
EnergyConsumption=54,55
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=495,8
Measure_Date=01.06.2020 0:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=19,5
Measure_Date=01.06.2020 0:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=6108,48
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=5924,4
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=3004,5
"
},
 new object[]  {"EnergyConsumptionByDevicesByHour;Часовой расход электроэнергии по учётам",
@"Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1480,95
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,075
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=830,25
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=367,65
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=389,475
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=297,45
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=341,325
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=18
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,475
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=18,675
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=579,96
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=67,308
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=49,164
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=34,932
Measure_Date=01.06.2020 0:00:00
Device_Name=АБК
EnergyConsumption=7,275
Measure_Date=01.06.2020 0:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=132,68
Measure_Date=01.06.2020 0:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 0:00:00
Device_Name=ПРС-2
EnergyConsumption=49,92
Measure_Date=01.06.2020 0:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,31
Measure_Date=01.06.2020 0:00:00
Device_Name=Столовая
EnergyConsumption=2,175
Measure_Date=01.06.2020 0:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,56
Measure_Date=01.06.2020 0:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 0:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=298,88
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=279
Measure_Date=01.06.2020 0:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=171,6
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1480,05
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=808,2
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=366,75
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=387,45
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=319,5
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=339,525
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=18
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,7
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=19,125
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=579,96
Measure_Date=01.06.2020 1:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=64,5
Measure_Date=01.06.2020 1:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=53,016
Measure_Date=01.06.2020 1:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=39,288
Measure_Date=01.06.2020 1:00:00
Device_Name=АБК
EnergyConsumption=6,925
Measure_Date=01.06.2020 1:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=132,68
Measure_Date=01.06.2020 1:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 1:00:00
Device_Name=ПРС-2
EnergyConsumption=58,36
Measure_Date=01.06.2020 1:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,41
Measure_Date=01.06.2020 1:00:00
Device_Name=Столовая
EnergyConsumption=2,15
Measure_Date=01.06.2020 1:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,56
Measure_Date=01.06.2020 1:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 1:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=295,68
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=261,3
Measure_Date=01.06.2020 1:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=110,7
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1495,575
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=814,5
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=365,85
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=387
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=305,775
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=339,975
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=18
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,7
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,925
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=577,8
Measure_Date=01.06.2020 2:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=48,42
Measure_Date=01.06.2020 2:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=51,288
Measure_Date=01.06.2020 2:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=37,38
Measure_Date=01.06.2020 2:00:00
Device_Name=АБК
EnergyConsumption=6,8
Measure_Date=01.06.2020 2:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=132,92
Measure_Date=01.06.2020 2:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 2:00:00
Device_Name=ПРС-2
EnergyConsumption=25,12
Measure_Date=01.06.2020 2:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,48
Measure_Date=01.06.2020 2:00:00
Device_Name=Столовая
EnergyConsumption=2,15
Measure_Date=01.06.2020 2:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,76
Measure_Date=01.06.2020 2:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 2:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=298,24
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=275,7
Measure_Date=01.06.2020 2:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=147,9
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1498,275
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,525
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=795,15
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=366,075
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=389,025
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=226,575
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=340,2
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=15,75
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,925
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,475
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=577,8
Measure_Date=01.06.2020 3:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=69,912
Measure_Date=01.06.2020 3:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=49,248
Measure_Date=01.06.2020 3:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=37,392
Measure_Date=01.06.2020 3:00:00
Device_Name=АБК
EnergyConsumption=6,825
Measure_Date=01.06.2020 3:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=132,88
Measure_Date=01.06.2020 3:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 3:00:00
Device_Name=ПРС-2
EnergyConsumption=23,56
Measure_Date=01.06.2020 3:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,54
Measure_Date=01.06.2020 3:00:00
Device_Name=Столовая
EnergyConsumption=2,05
Measure_Date=01.06.2020 3:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,68
Measure_Date=01.06.2020 3:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 3:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=296,64
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=260,1
Measure_Date=01.06.2020 3:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=105,6
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1497,15
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=807,3
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=369,675
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=395,775
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=295,875
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=342,675
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,925
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,025
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=575,64
Measure_Date=01.06.2020 4:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=56,916
Measure_Date=01.06.2020 4:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=53,844
Measure_Date=01.06.2020 4:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=41,328
Measure_Date=01.06.2020 4:00:00
Device_Name=АБК
EnergyConsumption=6,7
Measure_Date=01.06.2020 4:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133
Measure_Date=01.06.2020 4:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 4:00:00
Device_Name=ПРС-2
EnergyConsumption=61,16
Measure_Date=01.06.2020 4:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,42
Measure_Date=01.06.2020 4:00:00
Device_Name=Столовая
EnergyConsumption=2,225
Measure_Date=01.06.2020 4:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,72
Measure_Date=01.06.2020 4:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 4:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=298,24
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=259,2
Measure_Date=01.06.2020 4:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=98,4
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1498,05
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=816,3
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=366,75
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=395,325
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=307,8
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=342,675
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,7
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,25
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=576,18
Measure_Date=01.06.2020 5:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=34,968
Measure_Date=01.06.2020 5:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=51,108
Measure_Date=01.06.2020 5:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=43,116
Measure_Date=01.06.2020 5:00:00
Device_Name=АБК
EnergyConsumption=6,85
Measure_Date=01.06.2020 5:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133
Measure_Date=01.06.2020 5:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 5:00:00
Device_Name=ПРС-2
EnergyConsumption=54,64
Measure_Date=01.06.2020 5:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,34
Measure_Date=01.06.2020 5:00:00
Device_Name=Столовая
EnergyConsumption=2,1
Measure_Date=01.06.2020 5:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,76
Measure_Date=01.06.2020 5:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 5:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=290,24
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=230,1
Measure_Date=01.06.2020 5:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=114,3
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1505,7
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=812,025
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=369,9
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=400,05
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=299,7
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=345,375
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,15
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,475
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=19,125
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=572,4
Measure_Date=01.06.2020 6:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=30,96
Measure_Date=01.06.2020 6:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=31,068
Measure_Date=01.06.2020 6:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=34,356
Measure_Date=01.06.2020 6:00:00
Device_Name=АБК
EnergyConsumption=6,925
Measure_Date=01.06.2020 6:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,12
Measure_Date=01.06.2020 6:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 6:00:00
Device_Name=ПРС-2
EnergyConsumption=35,24
Measure_Date=01.06.2020 6:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,38
Measure_Date=01.06.2020 6:00:00
Device_Name=Столовая
EnergyConsumption=2,1
Measure_Date=01.06.2020 6:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,72
Measure_Date=01.06.2020 6:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 6:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=283,84
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=218,7
Measure_Date=01.06.2020 6:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=59,4
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1492,425
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=815,175
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=366,975
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=403,2
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=219,6
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=347,85
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,15
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=362,925
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=18,675
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=574,02
Measure_Date=01.06.2020 7:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=17,676
Measure_Date=01.06.2020 7:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=24,6
Measure_Date=01.06.2020 7:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=29,532
Measure_Date=01.06.2020 7:00:00
Device_Name=АБК
EnergyConsumption=8,625
Measure_Date=01.06.2020 7:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,28
Measure_Date=01.06.2020 7:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 7:00:00
Device_Name=ПРС-2
EnergyConsumption=44,44
Measure_Date=01.06.2020 7:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,25
Measure_Date=01.06.2020 7:00:00
Device_Name=Столовая
EnergyConsumption=2,525
Measure_Date=01.06.2020 7:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,72
Measure_Date=01.06.2020 7:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 7:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=283,84
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=209,1
Measure_Date=01.06.2020 7:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=44,1
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1498,05
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=833,85
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=362,025
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=402,3
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=258,3
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=346,95
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=364,05
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=21,375
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=570,78
Measure_Date=01.06.2020 8:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=22,848
Measure_Date=01.06.2020 8:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=16,86
Measure_Date=01.06.2020 8:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=28,236
Measure_Date=01.06.2020 8:00:00
Device_Name=АБК
EnergyConsumption=14,525
Measure_Date=01.06.2020 8:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,2
Measure_Date=01.06.2020 8:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 8:00:00
Device_Name=ПРС-2
EnergyConsumption=84,92
Measure_Date=01.06.2020 8:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25,14
Measure_Date=01.06.2020 8:00:00
Device_Name=Столовая
EnergyConsumption=2,725
Measure_Date=01.06.2020 8:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,72
Measure_Date=01.06.2020 8:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 8:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=283,84
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=234,9
Measure_Date=01.06.2020 8:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=64,8
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1514,025
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=819,225
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=369,9
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=394,65
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=284,4
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=344,7
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,075
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=25,2
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=569,16
Measure_Date=01.06.2020 9:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=69,264
Measure_Date=01.06.2020 9:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=48,336
Measure_Date=01.06.2020 9:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=34,5
Measure_Date=01.06.2020 9:00:00
Device_Name=АБК
EnergyConsumption=16
Measure_Date=01.06.2020 9:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,36
Measure_Date=01.06.2020 9:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 9:00:00
Device_Name=ПРС-2
EnergyConsumption=53,48
Measure_Date=01.06.2020 9:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,94
Measure_Date=01.06.2020 9:00:00
Device_Name=Столовая
EnergyConsumption=2,325
Measure_Date=01.06.2020 9:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 9:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 9:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=301,76
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=271,8
Measure_Date=01.06.2020 9:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=121,2
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1532,475
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,075
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=834,3
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=374,4
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=394,2
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=250,2
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=343,8
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,975
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=24,75
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=568,62
Measure_Date=01.06.2020 10:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=66,576
Measure_Date=01.06.2020 10:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=52,584
Measure_Date=01.06.2020 10:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=43,896
Measure_Date=01.06.2020 10:00:00
Device_Name=АБК
EnergyConsumption=15,3
Measure_Date=01.06.2020 10:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,52
Measure_Date=01.06.2020 10:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=Компрессор
EnergyConsumption=0
Measure_Date=01.06.2020 10:00:00
Device_Name=ПРС-2
EnergyConsumption=96,32
Measure_Date=01.06.2020 10:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,98
Measure_Date=01.06.2020 10:00:00
Device_Name=Столовая
EnergyConsumption=2,6
Measure_Date=01.06.2020 10:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,64
Measure_Date=01.06.2020 10:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,84
Measure_Date=01.06.2020 10:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=302,72
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=271,8
Measure_Date=01.06.2020 10:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=128,7
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1542,6
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,075
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=830,925
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=373,275
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=386,775
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=287,325
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=340,425
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,825
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=367,425
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=24,075
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=568,08
Measure_Date=01.06.2020 11:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=69,624
Measure_Date=01.06.2020 11:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=44,148
Measure_Date=01.06.2020 11:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 11:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=39,168
Measure_Date=01.06.2020 11:00:00
Device_Name=АБК
EnergyConsumption=15,425
Measure_Date=01.06.2020 11:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,6
Measure_Date=01.06.2020 11:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 11:00:00
Device_Name=Компрессор
EnergyConsumption=36
Measure_Date=01.06.2020 11:00:00
Device_Name=ПРС-2
EnergyConsumption=63,24
Measure_Date=01.06.2020 11:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=25
Measure_Date=01.06.2020 11:00:00
Device_Name=Столовая
EnergyConsumption=2,6
Measure_Date=01.06.2020 11:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 11:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 11:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=271,04
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=269,1
Measure_Date=01.06.2020 11:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=123,3
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1537,875
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=833,4
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=376,425
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=391,5
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=310,275
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=344,475
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=367,875
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=22,95
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=567
Measure_Date=01.06.2020 12:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=64,884
Measure_Date=01.06.2020 12:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=50,412
Measure_Date=01.06.2020 12:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 12:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=41,952
Measure_Date=01.06.2020 12:00:00
Device_Name=АБК
EnergyConsumption=16,175
Measure_Date=01.06.2020 12:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,64
Measure_Date=01.06.2020 12:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 12:00:00
Device_Name=Компрессор
EnergyConsumption=77,19
Measure_Date=01.06.2020 12:00:00
Device_Name=ПРС-2
EnergyConsumption=75,48
Measure_Date=01.06.2020 12:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,83
Measure_Date=01.06.2020 12:00:00
Device_Name=Столовая
EnergyConsumption=2,3
Measure_Date=01.06.2020 12:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,56
Measure_Date=01.06.2020 12:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 12:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=226,88
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=271,5
Measure_Date=01.06.2020 12:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=152,1
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1527,3
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,525
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=821,925
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=370,8
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=385,425
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=291,15
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=326,475
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,825
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=368,1
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=19,575
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=567
Measure_Date=01.06.2020 13:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=70,764
Measure_Date=01.06.2020 13:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=43,764
Measure_Date=01.06.2020 13:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 13:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=40,272
Measure_Date=01.06.2020 13:00:00
Device_Name=АБК
EnergyConsumption=15,45
Measure_Date=01.06.2020 13:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,72
Measure_Date=01.06.2020 13:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 13:00:00
Device_Name=Компрессор
EnergyConsumption=81,21
Measure_Date=01.06.2020 13:00:00
Device_Name=ПРС-2
EnergyConsumption=54,52
Measure_Date=01.06.2020 13:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,67
Measure_Date=01.06.2020 13:00:00
Device_Name=Столовая
EnergyConsumption=2,525
Measure_Date=01.06.2020 13:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 13:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 13:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=217,92
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=275,4
Measure_Date=01.06.2020 13:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=130,2
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1522,35
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=837,45
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=370,125
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=384,975
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=310,725
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=325,575
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=367,2
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,25
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=559,44
Measure_Date=01.06.2020 14:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=71,952
Measure_Date=01.06.2020 14:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=50,556
Measure_Date=01.06.2020 14:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 14:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=42,984
Measure_Date=01.06.2020 14:00:00
Device_Name=АБК
EnergyConsumption=14,2
Measure_Date=01.06.2020 14:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,84
Measure_Date=01.06.2020 14:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 14:00:00
Device_Name=Компрессор
EnergyConsumption=80,85
Measure_Date=01.06.2020 14:00:00
Device_Name=ПРС-2
EnergyConsumption=80,52
Measure_Date=01.06.2020 14:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,18
Measure_Date=01.06.2020 14:00:00
Device_Name=Столовая
EnergyConsumption=2,525
Measure_Date=01.06.2020 14:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 14:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 14:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=218,88
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=261,9
Measure_Date=01.06.2020 14:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=200,7
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1510,425
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=818,775
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=369,9
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=392,4
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=312,075
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=326,7
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,825
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,75
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=19,8
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=555,12
Measure_Date=01.06.2020 15:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=18,792
Measure_Date=01.06.2020 15:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=50,304
Measure_Date=01.06.2020 15:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 15:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=43,32
Measure_Date=01.06.2020 15:00:00
Device_Name=АБК
EnergyConsumption=14,3
Measure_Date=01.06.2020 15:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,84
Measure_Date=01.06.2020 15:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 15:00:00
Device_Name=Компрессор
EnergyConsumption=74,1
Measure_Date=01.06.2020 15:00:00
Device_Name=ПРС-2
EnergyConsumption=56,16
Measure_Date=01.06.2020 15:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,08
Measure_Date=01.06.2020 15:00:00
Device_Name=Столовая
EnergyConsumption=2
Measure_Date=01.06.2020 15:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 15:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 15:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=214,08
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=218,1
Measure_Date=01.06.2020 15:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=132
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1505,025
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=832,95
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=367,425
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=391,725
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=319,275
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=325,8
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,825
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,75
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=19,35
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=554,58
Measure_Date=01.06.2020 16:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=16,968
Measure_Date=01.06.2020 16:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=47,772
Measure_Date=01.06.2020 16:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 16:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=39,852
Measure_Date=01.06.2020 16:00:00
Device_Name=АБК
EnergyConsumption=13,775
Measure_Date=01.06.2020 16:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,72
Measure_Date=01.06.2020 16:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 16:00:00
Device_Name=Компрессор
EnergyConsumption=74,61
Measure_Date=01.06.2020 16:00:00
Device_Name=ПРС-2
EnergyConsumption=79,48
Measure_Date=01.06.2020 16:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,5
Measure_Date=01.06.2020 16:00:00
Device_Name=Столовая
EnergyConsumption=2,225
Measure_Date=01.06.2020 16:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,56
Measure_Date=01.06.2020 16:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 16:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=214,4
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=220,5
Measure_Date=01.06.2020 16:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=153,6
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1510,425
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=830,25
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=365,175
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=391,05
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=288,225
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=327,375
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,975
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=19,125
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=554,04
Measure_Date=01.06.2020 17:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=22,116
Measure_Date=01.06.2020 17:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=49,476
Measure_Date=01.06.2020 17:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 17:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=40,92
Measure_Date=01.06.2020 17:00:00
Device_Name=АБК
EnergyConsumption=11,9
Measure_Date=01.06.2020 17:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,92
Measure_Date=01.06.2020 17:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 17:00:00
Device_Name=Компрессор
EnergyConsumption=76,32
Measure_Date=01.06.2020 17:00:00
Device_Name=ПРС-2
EnergyConsumption=67,48
Measure_Date=01.06.2020 17:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,45
Measure_Date=01.06.2020 17:00:00
Device_Name=Столовая
EnergyConsumption=2,375
Measure_Date=01.06.2020 17:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 17:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 17:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=215,68
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=220,8
Measure_Date=01.06.2020 17:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=153,3
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1509,75
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,3
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=817,875
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=362,25
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=391,5
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=298,8
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=327,375
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,75
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=18,45
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=555,12
Measure_Date=01.06.2020 18:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=16,692
Measure_Date=01.06.2020 18:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=46,932
Measure_Date=01.06.2020 18:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 18:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=36,144
Measure_Date=01.06.2020 18:00:00
Device_Name=АБК
EnergyConsumption=8,275
Measure_Date=01.06.2020 18:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,76
Measure_Date=01.06.2020 18:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 18:00:00
Device_Name=Компрессор
EnergyConsumption=71,04
Measure_Date=01.06.2020 18:00:00
Device_Name=ПРС-2
EnergyConsumption=53,72
Measure_Date=01.06.2020 18:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,4
Measure_Date=01.06.2020 18:00:00
Device_Name=Столовая
EnergyConsumption=1,95
Measure_Date=01.06.2020 18:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,64
Measure_Date=01.06.2020 18:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 18:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=214,08
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=211,5
Measure_Date=01.06.2020 18:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=149,1
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1500,3
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=819
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=366,075
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=391,275
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=297,675
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=327,15
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,75
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=18,9
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=554,04
Measure_Date=01.06.2020 19:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=20,076
Measure_Date=01.06.2020 19:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=23,64
Measure_Date=01.06.2020 19:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 19:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=16,14
Measure_Date=01.06.2020 19:00:00
Device_Name=АБК
EnergyConsumption=8,2
Measure_Date=01.06.2020 19:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,76
Measure_Date=01.06.2020 19:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 19:00:00
Device_Name=Компрессор
EnergyConsumption=68,97
Measure_Date=01.06.2020 19:00:00
Device_Name=ПРС-2
EnergyConsumption=40,28
Measure_Date=01.06.2020 19:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,46
Measure_Date=01.06.2020 19:00:00
Device_Name=Столовая
EnergyConsumption=2,25
Measure_Date=01.06.2020 19:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,6
Measure_Date=01.06.2020 19:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 19:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=212,8
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=224,7
Measure_Date=01.06.2020 19:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=97,5
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1506,6
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,075
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=811,575
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=365,4
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=391,95
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=278,325
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=326,925
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=12,6
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,525
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,475
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=556,2
Measure_Date=01.06.2020 20:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=37,464
Measure_Date=01.06.2020 20:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=22,344
Measure_Date=01.06.2020 20:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 20:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=28,176
Measure_Date=01.06.2020 20:00:00
Device_Name=АБК
EnergyConsumption=8,475
Measure_Date=01.06.2020 20:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,76
Measure_Date=01.06.2020 20:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 20:00:00
Device_Name=Компрессор
EnergyConsumption=77,67
Measure_Date=01.06.2020 20:00:00
Device_Name=ПРС-2
EnergyConsumption=47,36
Measure_Date=01.06.2020 20:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,48
Measure_Date=01.06.2020 20:00:00
Device_Name=Столовая
EnergyConsumption=2,25
Measure_Date=01.06.2020 20:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,68
Measure_Date=01.06.2020 20:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,8
Measure_Date=01.06.2020 20:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=216,96
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=240,3
Measure_Date=01.06.2020 20:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=98,4
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1493,775
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=24,075
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=820,8
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=363,15
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=392,175
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=288,675
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=329,625
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=13,275
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,975
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=22,05
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=561,6
Measure_Date=01.06.2020 21:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=36,504
Measure_Date=01.06.2020 21:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=37,356
Measure_Date=01.06.2020 21:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 21:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=39,792
Measure_Date=01.06.2020 21:00:00
Device_Name=АБК
EnergyConsumption=7,175
Measure_Date=01.06.2020 21:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=133,92
Measure_Date=01.06.2020 21:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 21:00:00
Device_Name=Компрессор
EnergyConsumption=76,29
Measure_Date=01.06.2020 21:00:00
Device_Name=ПРС-2
EnergyConsumption=47,48
Measure_Date=01.06.2020 21:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,49
Measure_Date=01.06.2020 21:00:00
Device_Name=Столовая
EnergyConsumption=1,975
Measure_Date=01.06.2020 21:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,76
Measure_Date=01.06.2020 21:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 21:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=215,04
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=230,4
Measure_Date=01.06.2020 21:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=106,2
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1503,675
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=832,05
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=365,85
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=392,175
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=256,05
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=328,95
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=18,225
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,525
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=21,15
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=568,08
Measure_Date=01.06.2020 22:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=44,076
Measure_Date=01.06.2020 22:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=48,204
Measure_Date=01.06.2020 22:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 22:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=42,264
Measure_Date=01.06.2020 22:00:00
Device_Name=АБК
EnergyConsumption=7,2
Measure_Date=01.06.2020 22:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=134,16
Measure_Date=01.06.2020 22:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 22:00:00
Device_Name=Компрессор
EnergyConsumption=80,97
Measure_Date=01.06.2020 22:00:00
Device_Name=ПРС-2
EnergyConsumption=39,88
Measure_Date=01.06.2020 22:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,51
Measure_Date=01.06.2020 22:00:00
Device_Name=Столовая
EnergyConsumption=2,225
Measure_Date=01.06.2020 22:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,76
Measure_Date=01.06.2020 22:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 22:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=218,88
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=256,8
Measure_Date=01.06.2020 22:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=128,7
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 12 (БДМ-2)
EnergyConsumption=1590,75
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 13 (Мак.уч)
EnergyConsumption=23,85
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 14(БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 15 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 17 (БДМ-2)
EnergyConsumption=857,925
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 18 (БДМ-1)
EnergyConsumption=0
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 19 (БДМ-1)
EnergyConsumption=348,975
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 20 (БДМ-1 привод)
EnergyConsumption=392,4
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 21 (БДМ-1)
EnergyConsumption=204,3
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 22 (БДМ-1 привод)
EnergyConsumption=330,075
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 28 (Мак.уч.)
EnergyConsumption=18
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20  Ячейка 30 (БДМ-1)
EnergyConsumption=366,975
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20 Ячейка 24 (Склад, зар-е)
EnergyConsumption=20,475
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_21_яч №5
EnergyConsumption=0
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_21_яч. №6
EnergyConsumption=568,08
Measure_Date=01.06.2020 23:00:00
Device_Name=ТП_59 (34_CMG)
EnergyConsumption=54,504
Measure_Date=01.06.2020 23:00:00
Device_Name=ТП_59 (35_SDF)
EnergyConsumption=54,072
Measure_Date=01.06.2020 23:00:00
Device_Name=ТП_59 (36_S4)
EnergyConsumption=0
Measure_Date=01.06.2020 23:00:00
Device_Name=ТП_59 (37_X5)
EnergyConsumption=37,356
Measure_Date=01.06.2020 23:00:00
Device_Name=АБК
EnergyConsumption=6,925
Measure_Date=01.06.2020 23:00:00
Device_Name=Вакуумный насос 41 М БДМ-1
EnergyConsumption=134,32
Measure_Date=01.06.2020 23:00:00
Device_Name=Вакуумный насос 42 М БДМ-1
EnergyConsumption=0
Measure_Date=01.06.2020 23:00:00
Device_Name=Компрессор
EnergyConsumption=80,31
Measure_Date=01.06.2020 23:00:00
Device_Name=ПРС-2
EnergyConsumption=55,2
Measure_Date=01.06.2020 23:00:00
Device_Name=Пылеудаление ПРС-2
EnergyConsumption=24,51
Measure_Date=01.06.2020 23:00:00
Device_Name=Столовая
EnergyConsumption=2,225
Measure_Date=01.06.2020 23:00:00
Device_Name=ТП-59: Панель вентиляции
EnergyConsumption=20,8
Measure_Date=01.06.2020 23:00:00
Device_Name=БП_82 (ПРС-1)
EnergyConsumption=0,82
Measure_Date=01.06.2020 23:00:00
Device_Name=КТП-БДМ_2 (БДМ-1 вак.насосы)
EnergyConsumption=217,92
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20 Ячейка 16 (СГИ)
EnergyConsumption=251,7
Measure_Date=01.06.2020 23:00:00
Device_Name=РП_20_яч. №11
EnergyConsumption=212,7
"
},
 new object[]  {"SkuDataByShifts_BDM1;По сменный отчет БДМ-1",
@"Sku=БОТ Ц-84 код A БЕЛЫЙ
ShiftId=3
CycleDateBegin=31.05.2020 20:00:00
CycleDateEnd=01.06.2020 7:59:59
Machinist_name=Протопопов.А.С
FluidStream=52135,981
Wetness=6,582
ShiftProductivity=30860
EnergyConsumptionPerTonne=1,047
SteamConsumptionPerTonne=0,8
GasConsumptionPerTonne=0,16
AverageSpeed=1465
Sku=БОТ Ц-84 код A БЕЛЫЙ
ShiftId=1
CycleDateBegin=01.06.2020 8:00:00
CycleDateEnd=01.06.2020 19:59:59
Machinist_name=Русских С.О.
FluidStream=50840,562
Wetness=6,667
ShiftProductivity=31150
EnergyConsumptionPerTonne=1,041
SteamConsumptionPerTonne=0,767
GasConsumptionPerTonne=0,154
AverageSpeed=1467
Sku=БОТ Ц-84 код A БЕЛЫЙ
ShiftId=4
CycleDateBegin=01.06.2020 20:00:00
CycleDateEnd=02.06.2020 7:59:59
Machinist_name=Ивонин Д.Ю.
FluidStream=36874,767
Wetness=6,86
ShiftProductivity=21760
EnergyConsumptionPerTonne=1,142
SteamConsumptionPerTonne=0,954
GasConsumptionPerTonne=0,182
AverageSpeed=1174
IdStgPlanTer=6
PlaceId=1
Sku=БОТ Ц-84 код A БЕЛЫЙ
Year=2020
Month=6
BoValue=0
ShiftProductivityMin=36000
ShiftProductivityNorm=40000
EnergyPerTonneMin=0,912
EnergyPerTonneNorm=1,013
SteamPerTonneMin=0,652
SteamPerTonneNorm=0,724
GasPerTonneMin=0,136
GasPerTonneNorm=0,151
AvarageSpeedMin=1260
AvarageSpeedNorm=1400
0
1
2
31.05.2020 20:00-01.06.2020 7:59
01.06.2020 8:00-01.06.2020 19:59
01.06.2020 20:00-02.06.2020 7:59
"
},
 new object[]  {"ConsumptionByBDM2ByHour;БДМ-2(Часовой)",
@"StartPeriod=01.06.2020 0:00:00
EndPeriod=01.06.2020 1:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,94
GasConsumption=0,517
SteamConsumption=3,26
AverageSpeed=1630
StartPeriod=01.06.2020 1:00:00
EndPeriod=01.06.2020 2:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,91
GasConsumption=0,517
SteamConsumption=3,26
AverageSpeed=1638
StartPeriod=01.06.2020 2:00:00
EndPeriod=01.06.2020 3:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,96
GasConsumption=0,515
SteamConsumption=3,28
AverageSpeed=1648
StartPeriod=01.06.2020 3:00:00
EndPeriod=01.06.2020 4:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,95
GasConsumption=0,517
SteamConsumption=3,27
AverageSpeed=1650
StartPeriod=01.06.2020 4:00:00
EndPeriod=01.06.2020 5:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,92
GasConsumption=0,515
SteamConsumption=3,26
AverageSpeed=1652
StartPeriod=01.06.2020 5:00:00
EndPeriod=01.06.2020 6:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,94
GasConsumption=0,516
SteamConsumption=3,27
AverageSpeed=1651
StartPeriod=01.06.2020 6:00:00
EndPeriod=01.06.2020 7:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,97
GasConsumption=0,517
SteamConsumption=3,28
AverageSpeed=1645
StartPeriod=01.06.2020 7:00:00
EndPeriod=01.06.2020 8:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,95
GasConsumption=0,518
SteamConsumption=3,27
AverageSpeed=1641
StartPeriod=01.06.2020 8:00:00
EndPeriod=01.06.2020 9:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,94
GasConsumption=0,514
SteamConsumption=3,26
AverageSpeed=1650
StartPeriod=01.06.2020 9:00:00
EndPeriod=01.06.2020 10:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,95
GasConsumption=0,516
SteamConsumption=3,26
AverageSpeed=1650
StartPeriod=01.06.2020 10:00:00
EndPeriod=01.06.2020 11:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,94
GasConsumption=0,514
SteamConsumption=3,26
AverageSpeed=1674
StartPeriod=01.06.2020 11:00:00
EndPeriod=01.06.2020 12:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,98
GasConsumption=0,517
SteamConsumption=3,27
AverageSpeed=1678
StartPeriod=01.06.2020 12:00:00
EndPeriod=01.06.2020 13:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,97
GasConsumption=0,517
SteamConsumption=3,27
AverageSpeed=1670
StartPeriod=01.06.2020 13:00:00
EndPeriod=01.06.2020 14:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,97
GasConsumption=0,516
SteamConsumption=3,27
AverageSpeed=1660
StartPeriod=01.06.2020 14:00:00
EndPeriod=01.06.2020 15:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,96
GasConsumption=0,516
SteamConsumption=3,27
AverageSpeed=1660
StartPeriod=01.06.2020 15:00:00
EndPeriod=01.06.2020 16:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,96
GasConsumption=0,517
SteamConsumption=3,26
AverageSpeed=1660
StartPeriod=01.06.2020 16:00:00
EndPeriod=01.06.2020 17:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,94
GasConsumption=0,516
SteamConsumption=3,26
AverageSpeed=1660
StartPeriod=01.06.2020 17:00:00
EndPeriod=01.06.2020 18:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,96
GasConsumption=0,516
SteamConsumption=3,26
AverageSpeed=1660
StartPeriod=01.06.2020 18:00:00
EndPeriod=01.06.2020 19:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,96
GasConsumption=0,516
SteamConsumption=3,26
AverageSpeed=1660
StartPeriod=01.06.2020 19:00:00
EndPeriod=01.06.2020 20:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,97
GasConsumption=0,516
SteamConsumption=3,24
AverageSpeed=1656
StartPeriod=01.06.2020 20:00:00
EndPeriod=01.06.2020 21:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,95
GasConsumption=0,515
SteamConsumption=3,25
AverageSpeed=1650
StartPeriod=01.06.2020 21:00:00
EndPeriod=01.06.2020 22:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,95
GasConsumption=0,516
SteamConsumption=3,23
AverageSpeed=1642
StartPeriod=01.06.2020 22:00:00
EndPeriod=01.06.2020 23:00:00
Place=БДМ-2
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=1,97
GasConsumption=0,516
SteamConsumption=3,24
AverageSpeed=1645
"
},
 new object[]  {"ConsumptionByBDM1ByHour;БДМ-1(Часовой)",
@"StartPeriod=01.06.2020 0:00:00
EndPeriod=01.06.2020 1:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,78
GasConsumption=0,411
SteamConsumption=2,11
AverageSpeed=1460
StartPeriod=01.06.2020 1:00:00
EndPeriod=01.06.2020 2:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,72
GasConsumption=0,411
SteamConsumption=2,04
AverageSpeed=1460
StartPeriod=01.06.2020 2:00:00
EndPeriod=01.06.2020 3:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,77
GasConsumption=0,412
SteamConsumption=2,01
AverageSpeed=1460
StartPeriod=01.06.2020 3:00:00
EndPeriod=01.06.2020 4:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,61
GasConsumption=0,412
SteamConsumption=2,02
AverageSpeed=1470
StartPeriod=01.06.2020 4:00:00
EndPeriod=01.06.2020 5:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,69
GasConsumption=0,412
SteamConsumption=2,02
AverageSpeed=1470
StartPeriod=01.06.2020 5:00:00
EndPeriod=01.06.2020 6:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,71
GasConsumption=0,412
SteamConsumption=2,03
AverageSpeed=1470
StartPeriod=01.06.2020 6:00:00
EndPeriod=01.06.2020 7:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,67
GasConsumption=0,412
SteamConsumption=2,03
AverageSpeed=1480
StartPeriod=01.06.2020 7:00:00
EndPeriod=01.06.2020 8:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,59
GasConsumption=0,412
SteamConsumption=2,03
AverageSpeed=1480
StartPeriod=01.06.2020 8:00:00
EndPeriod=01.06.2020 9:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,67
GasConsumption=0,41
SteamConsumption=2,04
AverageSpeed=1471
StartPeriod=01.06.2020 9:00:00
EndPeriod=01.06.2020 10:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,7
GasConsumption=0,401
SteamConsumption=2,04
AverageSpeed=1470
StartPeriod=01.06.2020 10:00:00
EndPeriod=01.06.2020 11:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,67
GasConsumption=0,4
SteamConsumption=2,05
AverageSpeed=1464
StartPeriod=01.06.2020 11:00:00
EndPeriod=01.06.2020 12:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,69
GasConsumption=0,4
SteamConsumption=2,04
AverageSpeed=1463
StartPeriod=01.06.2020 12:00:00
EndPeriod=01.06.2020 13:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,75
GasConsumption=0,4
SteamConsumption=2
AverageSpeed=1467
StartPeriod=01.06.2020 13:00:00
EndPeriod=01.06.2020 14:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,69
GasConsumption=0,399
SteamConsumption=1,99
AverageSpeed=1460
StartPeriod=01.06.2020 14:00:00
EndPeriod=01.06.2020 15:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,75
GasConsumption=0,399
SteamConsumption=2
AverageSpeed=1466
StartPeriod=01.06.2020 15:00:00
EndPeriod=01.06.2020 16:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,69
GasConsumption=0,399
SteamConsumption=1,99
AverageSpeed=1470
StartPeriod=01.06.2020 16:00:00
EndPeriod=01.06.2020 17:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,73
GasConsumption=0,398
SteamConsumption=1,95
AverageSpeed=1470
StartPeriod=01.06.2020 17:00:00
EndPeriod=01.06.2020 18:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,69
GasConsumption=0,398
SteamConsumption=1,93
AverageSpeed=1470
StartPeriod=01.06.2020 18:00:00
EndPeriod=01.06.2020 19:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,7
GasConsumption=0,398
SteamConsumption=1,95
AverageSpeed=1470
StartPeriod=01.06.2020 19:00:00
EndPeriod=01.06.2020 20:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,7
GasConsumption=0,399
SteamConsumption=1,92
AverageSpeed=1470
StartPeriod=01.06.2020 20:00:00
EndPeriod=01.06.2020 21:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,67
GasConsumption=0,399
SteamConsumption=1,9
AverageSpeed=1470
StartPeriod=01.06.2020 21:00:00
EndPeriod=01.06.2020 22:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,67
GasConsumption=0,399
SteamConsumption=1,91
AverageSpeed=1470
StartPeriod=01.06.2020 22:00:00
EndPeriod=01.06.2020 23:00:00
Place=БДМ-1
Composition=БОТ Ц-84 код A БЕЛЫЙ
EnergyConsumption=2,67
GasConsumption=0,4
SteamConsumption=1,93
AverageSpeed=1470
"
}
		};

		public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

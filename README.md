﻿# OsEngine
Архитектура проекта OSEngine в Enterprise Architect  
https://drive.google.com/drive/folders/1FA7I1rVR9tt-RobxoRGPnM7gPgNIhlsy

[_Lua Quik {Tarantool} & {Facebook.Tourch} for Enterprise Architect] Исходный код и UML-диаграмма классов  
https://drive.google.com/drive/folders/19Hykk1knOzqPPVhdEJ0CJtDeHV-v-3Gu

Подключение OsEngine к Квик при помощи LUA коннектора  
https://www.youtube.com/watch?v=bInNPjChvEg

Trailing Stop в OsEngine  
https://www.youtube.com/watch?v=jCDg081ayJY

![oslogo250](https://cloud.githubusercontent.com/assets/26077466/23395381/5545b688-fd9f-11e6-8db9-c8e8944a8cc2.png)

# Open Source Algo Trading Platform

[Сайт разработчиков](http://o-s-a.net)

Ну и поскольку она для СНГ и пишется исключительно с русскими комментариями, далее всё на великом...

## Что такое OsEngine?

Это полный комплекс программ необходимых для автоматизации торговли на бирже. 

![default](https://user-images.githubusercontent.com/26077466/42362896-01b3e74a-80fe-11e8-8f36-3db24cb7522c.png)

В него входят:

*Os.Data* - программа для загрузки исторических данных, с помощью которой Вы можете получать свечи, стаканы и тики из самых различных источников.

*Os.Optimizer* - программа для тестирования на истории одной стратегии с разными параметрами.

*Os.Tester* - программа для тестирования на истории множества стратегий одновременно, но без перебора параметров. Поддерживает трансляцию нескольких таймфреймов и нескольких инструментов одновременно.

*Os.Miner* - программа для поиска прибыльных формаций на графике. Как в ручном, так и в автоматическом режиме. Работа с БигДатой у Вас на компьютере. Паттерны найденные при помощи этой программы можно запускать в торговлю.

*Os.Trader* - программа для торговли на бирже. 

Слой создания роботов у нас работает как с Os.Tester так и с Os.Trader без измнений. Он очень прост. Мы не изменяем его с каждым релизом и поддерживаем в нём обратную совместимость.

*Одинадцать подключений на сегодня*

1) Квик 

    а) DDE. Старое и хардовое подключение, проверенное временем.
	
    б) LUA. Новое и технологичное подключение. Шлём лучи поддержки: https://github.com/finsight/QUIKSharp, которые принимали участие в создании подключения своим кодом.
	
2) СмартКом
3) Плаза 2
4) Interactiv Brokers
5) Ninja Trader
6) ASTS Bridge, он же Micex TEAP
7) Bitmex - биржа криптоВалют
8) Kraken - биржа криптоВалют
9) BitStamp - биржа криптоВалют
10) Binance - биржа криптоВалют
11) OANDA - форекс

*В комплекте с OsEngine идёт более 30ти встроенных роботов*

1) классические трендовые роботы, вроде пересечения машек, стратегии Билла Вильямса или трендовой стратегии Джесси Ливермора.

2) контрТрендовые системы на боллинжере, линиях баланса и даже некоторые маркет-мэйкерские стратегии.

3) арбитражные стратегии для торговли расхождения коррелирующих инструментов, в том числе одноногие арбитражи.

Как пользоваться и писать ботов смотрите на канале: https://www.youtube.com/channel/UCLmOUsdFs48mo37hgXmIJTQ/videos

Форум: http://o-s-a.net/forum



# OsEngine

![oslogo250](https://cloud.githubusercontent.com/assets/26077466/23395381/5545b688-fd9f-11e6-8db9-c8e8944a8cc2.png)

# Open Source Algo Trading Platform

[Developer site](http://o-s-a.net/eng/)

## What is OsEngine?

This is a full range of programs required to automate trading on the stock exchange.

![algotasks](https://user-images.githubusercontent.com/26077466/53003898-e9a84a00-3427-11e9-9582-53dd1a18271a.png)

It includes:

The layer for creating robots is similar to the Wealth-Lab script and Ninja Script. It's simple. We do not change it with every release and support backward compatibility.

*Data* - program to download historical data. With the program you can get candles, market depths and trades from a variety of sources.

*Optimizer* - the program to select the optimal parameters for the strategy.

*Tester* - exchange emulator. The program for testing on the history of many strategies at the same time, with a single portfolio. Supports translation of multiple timeframes and multiple instruments at the same time.

*Miner* - the program to search for profitable formations on the chart. Both manual and automatic. Work with Bigdata on your computer. Patterns found with the help of this program can be launched into trading.

*Bot station* -the program to run the robots in the trade.


*Available international connections*

1) LMAX
2) Interactiv Brokers
3) Ninja trader

*Available connections for MOEX*

1) Quik
2) SmartCom
3) Transaq
4) Plaza 2
5) Asts Bridge

*Available connections for cryptocurrency exchanges*

1) Binance
2) Bitmex
3) Bitstamp
4) Bitfinex
5) Kraken
6) BitMax
7) LiveCoin
8) ExMo
9) ZB

*Available connections for Forex*

1) OANDA


*Included with OsEngine is more than 30 built-in robots*

1) Сlassic trend robots like moving average crossing, bill Williams strategy or Jesse Livermore trend strategy.

2) Counter-trend systems on Bollinger bands, balance lines and even some market-making strategies.

3) Arbitrage strategies for trading divergences of correlating instruments, including one-legged arbitrage.

Forum http://o-s-a.net/eng/forum



![oslogo250](https://cloud.githubusercontent.com/assets/26077466/23395381/5545b688-fd9f-11e6-8db9-c8e8944a8cc2.png)

# Open Source Algo Trading Platform


## Что такое OsEngine?

Это полный комплекс программ необходимых для автоматизации торговли на бирже. 

[Сайт разработчиков](http://o-s-a.net)

![default](https://user-images.githubusercontent.com/26077466/42362896-01b3e74a-80fe-11e8-8f36-3db24cb7522c.png)

В него входят:

Слой создания роботов похожий на Wealth-Lab script и Ninja script. Он очень прост. Мы не изменяем его с каждым релизом и поддерживаем в нём обратную совместимость.

*OData* - программа для загрузки исторических данных, с помощью которой Вы можете получать свечи, стаканы и тики из самых различных источников.

*Optimizer* - программа для подбора оптимальных параметров для стратегии.

*Tester* - эмулятор биржи. Программа для тестирования на истории множества стратегий одновременно, с единым портфелем.  Поддерживает трансляцию нескольких таймфреймов и нескольких инструментов одновременно.

*Miner* - программа для поиска прибыльных формаций на графике. Как в ручном, так и в автоматическом режиме. Работа с БигДатой у Вас на компьютере. Паттерны найденные при помощи этой программы можно запускать в торговлю.

*Bot station* - программа для запуска роботов в торговлю.


*Доступные международные подключения*

1) LMAX
2) Interactiv Brokers
3) Ninja trader

*Доступные подключения для MOEX*

1) Quik
2) SmartCom
3) Transaq
4) Plaza 2
5) Asts Bridge

*Доступные подключения для бирж криптовалют*

1) Binance
2) Bitmex
3) Bitstamp
4) Bitfinex
5) Kraken
6) BitMax
7) LiveCoin
8) ExMo
9) ZB

*Доступные подключения для форекс*

1) OANDA


*В комплекте с OsEngine идёт более 30ти встроенных роботов*

1) классические трендовые роботы, вроде пересечения машек, стратегии Билла Вильямса или трендовой стратегии Джесси Ливермора.

2) контрТрендовые системы на боллинжере, линиях баланса и даже некоторые маркет-мэйкерские стратегии.

3) арбитражные стратегии для торговли расхождения коррелирующих инструментов, в том числе одноногие арбитражи.

Как пользоваться и писать ботов смотрите на канале: https://www.youtube.com/channel/UCLmOUsdFs48mo37hgXmIJTQ/videos

Форум: http://o-s-a.net/forum



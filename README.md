# Tower Defense
Проект по курсу "Проектирование на C#"

Команда: Уланов И.А, Орлов И.А. КН-201.

Суть проекта: Реализация многопользовательской версии игры Tower Defense. Генерируется симметричная карта, в углах карты базы двух игроков. Каждый игрок имеет стартовые деньги, за которые может строить башни на своей половине карты и покупать юнитов на своей базе. В течение игры, игрок может направлять юнитов. Победитель - тот, кто дойдет на вражеской базы. Визуализиция сделана средствами Unity. Игроками управляют боты с разными алгоритмами.

Скриншоты:
<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/Game1.png?raw=true">
<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/Game2.png?raw=true">
<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/Game3.png?raw=true">

Структура проекта:
```
/Game/Assets/Scripts/Model - модель игры
/Game/Assets/Scripts/View - отображение игры в Unity.
/Game/Assets/Scripts/Controller - контроллер наблюдателя.
/Game/Assets/Scripts/Bots - алгоритмы ботов.
/Game/Assets/Resources - префабы динамических объектов, создаваемых в процессе игры.
/Game/Assets/Scenes/Main.unity - сцена, настроенная для создания и отображения игры.
/Tests - модульные тесты на модель игры
```

Точки расширения функционала:
```
1. Дополнительные алгоритмы ботов. Реализовав абстрактный класс Bot, можно использовать другие алгоритмы.
2. Дополнительные игроковые объекты. Реализовав класс AbstartObject, можно добавлять в модель игры новые объекты.
3. Модель игры не использует Unity (физические модели, триггеры и прочие компоненты), и может быть без проблем визуализированно другими средствами.
```

Управление зависимостями:
Ввиду использование Unity и наличие в нем собственной системы зависимостей, основанной на компонентах и переопределяемых публичных полях, необходимость использования DI-контейнера пропадает.

<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/DI1.png?raw=true">
<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/DI2.png?raw=true">
<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/DI3.png?raw=true">
<img src="https://github.com/ulanzetz/Tower-Defense/raw/master/Readme-Images/DI4.png?raw=true">

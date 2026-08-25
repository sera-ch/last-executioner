# 🚀 Coral Conqueror Khann
A mod for the game Hollow Knight: Silksong that features a rematch with an enhanced Last Judge.

---

## 🛠 Features

- **Stats buff** - Increased boss speed and decreased attack intervals.
- **Changed attacks** - Some attacks receive a makeover.
- **New boss and area titles** - Changed the boss name (Last Executioner)

## 📦 Installation

Prerequisites: This mod requires `BepInEx`.

## 🚀 Usage
- Create a new `Directory.Build.targets` file inside the project directory:
```xml
<Project>
    <PropertyGroup>
        <SILKSONG_PATH>/home/your-name/snap/steam/common/.local/share/Steam/steamapps/common/Hollow Knight Silksong</SILKSONG_PATH>
    </PropertyGroup>
    <PropertyGroup>
        <DEBUG>true</DEBUG>
    </PropertyGroup>
</Project>

```
- Directory schema:
```
LastExecutioner
    ├ LastExecutioner.csproj
    └ Directory.Build.targets
```
- Run with `dotnet build`
- Place `LastExecutioner.dll` inside `/Hollow Knight Silksong/BepInEx/plugins`
```
Hollow Knight Silksong
├ Hollow Knight Silksong_Data
└ BepInEx
    └ plugins
        └ LastExecutioner.dll
```
- Launch the game
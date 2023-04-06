# Aris Studio Installer Resources

This folder contain resources to build Aris Studio using [Inno Setup](https://jrsoftware.org/ "Inno Setup")

## How to Build an Installer

- Download and install [Inno Setup](https://jrsoftware.org/isdl.php "Inno Setup").
- Open [Aris Studio Installer Script](../ArisStudio_Installer_Script.iss "Aris Studio Installer Script") in Inno Setup Compiler.
- Change these lines value (The description for those is on the file):

  ```pascal
  #define AppVersion ""
  ```

  ```pascal
  #define BuildPath ""
  ```

  ```pascal
  #define ResourcesPath ""
  ```

- Then Build. Go to `Build > Compile` or `Ctrl+F9`.

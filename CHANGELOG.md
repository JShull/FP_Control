# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.3.0] - 2022-11-08

### 0.3.0 Added

- [@JShull](https://github.com/jshull)
- The Scriptable object to include a boolean that allows you to ignore or set the player position from that data

### 0.3.0 Changed

- [@JShull](https://github.com/jshull)
- Samples folder is now hidden
- Adjusted namespace to better represent naming conventions

### 0.3.0 Fixed

- [@JShull](https://github.com/jshull)
- asmdef file is now in the root samples folder to allow for additional samples later without messing with asmdef files
- swapped the layers on the samples folder to include default unity layers, shouldn't have used custom layer names because Unity only provides 32 and this gets converted to an int which could cause confusion for newer unity users. Now just using Default and ignore raycast

## [0.2.0] - 2022-11-02

### 0.2.0 Added

- [@JShull](https://github.com/jshull)

### 0.2.0 Changed

- [@JShull](https://github.com/jshull)
- Modified the Scriptable object to include all of the data for the Unity Character Controller

### 0.2.0 Fixed

- Removed some of the log messages leftover

## [0.1.0] - 2022-11-01

### 0.1.0 Added

- [@JShull](https://github.com/jshull).
  - Moved all test files to a Unity Package Distribution
  - Setup the ChangeLog.md
  - Setup the Package Layout according to [Unity cus-Layout](https://docs.unity3d.com/Manual/cus-layout.html)
  - Humble Design has been setup with the PlayerController.cs script
    - Imported Samples provide the remaining Humble setup with the FP_Controller.cs script
  - See Samples in the Unity Package Manager to import an example
  - FP_Controller uses both old and new input - dependency on new input but you can swap the project to only use the old one and it will work
  - Scriptable Object setup to work with all other FuzzPhyte packages.
  - Added LICENSE.MD under a dual license for education and for business use cases

### 0.1.0 Changed

- None... yet

### 0.1.0 Fixed

- Setup the contents to align with Unity naming conventions

### 0.1.0 Removed

- None... yet
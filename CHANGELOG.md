# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.5.0] - 2024-2-4

### 0.5.0 Changed

- [@JShull](https://github.com/jshull)
- PlayerController.cs
  - The Move function now returns the move Vector3 data for debugging purposes
  - the Move function has a ceiling boolean parameter now being passed to it, previous code should still work as by default it's set to false
- URP Examples FP_Controller.cs
  - Uses the updated PlayerController.cs Move Ceiling parameter to have the sample reflect an accurate ceiling collision

### 0.5.0 Fixed

- [@JShull](https://github.com/jshull)
- Added in a ceiling check - this fixes the issue where players would jump and 'ground collide' with an item above them, ultimately being "stuck to the ceiling"
- The user still needs to do these grounds/ceiling checks on the Unity side but when passed to the PlayerController it will correctly function as intended
- Fixed an ever growing Y velocity on the move Vector being calculated by the PlayerController, this will now always reset to -1 if the player is grounded
  - this has a cap now: terminal velocity for a human -53m a second by whatever the gravity scaler is and 1000f on the upper bounds
  - this will fix the issue of if you have a player fall a great distance and not have you reach floating point hell

## [0.4.0] - 2023-11-12

### 0.4.0 Changed

- [@JShull](https://github.com/jshull)
  - Cleaning up Samples Folder
  - Correct References for URP
  - Modified Package.json to match
  - Modified *.asmdef to match requirements

## [0.3.5] - 2023-03-15

### 0.3.5 Added

- [@JShull](https://github.com/jshull)
- Modified the PlayerController.cs script to allow for actual derived classes to take advantage of the private/protected variables

### 0.3.5 Changed

- [@JShull](https://github.com/jshull)
- All private variables on PlayerController.cs are now protected

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

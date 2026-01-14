# Weather Toast Notification Package

A cross-platform Unity package that provides native toast notifications for Android and iOS platforms. Perfect for displaying weather information and other quick messages to users.

## Package Information

- **Package Name:** com.weather.toast
- **Version:** 1.0.0
- **Unity Version:** 2019.4+
- **Platforms Supported:** Android, iOS, Unity Editor

## Features

- Native Android Toast - Uses android.widget.Toast for authentic Android UI
- Native iOS Alert - Uses UIAlertController for standard iOS dialogs
- Cross-Platform - Automatically detects platform and uses appropriate native API
- Simple API - Single method call to show notifications
- No Dependencies - Works out of the box with Unity
- Editor Support - Falls back to Debug.Log for testing in Unity Editor

## Installation

### Method 1: Unity Package Manager (via disk)
1. Open Unity Editor
2. Go to Window → Package Manager
3. Click + → Add package from disk
4. Navigate to `Packages/com.weather.toast/package.json`
5. Click Open

### Method 2: Manual Installation
1. Copy the `com.weather.toast` folder into your project's `Packages` directory
2. Unity will automatically detect and import the package

## Usage

### Basic Usage

```csharp
using UnityEngine;
using WeatherToast;

public class MyApp : MonoBehaviour
{
    private WeatherToast.ToastController toastController;

    void Start()
    {
        toastController = gameObject.AddComponent<WeatherToast.ToastController>();
    }

    public void ShowMessage()
    {
        toastController.ShowToast("Your message here");
    }

    public void ShowMessageWithTitle()
    {
        toastController.ShowToast("Title", "Your message here");
    }
}
```

### Integration with UI Button

1. Create a GameObject in your scene
2. Add a script that uses WeatherToast.ToastController
3. Create a UI Button
4. In the Button's OnClick() event:
   - Drag the GameObject with your script
   - Select the method that calls ShowToast()

## Architecture

### Design Pattern: Platform Abstraction

The package follows clean architecture principles:

```
┌─────────────────────────────────────────┐
│         ToastController                 │
│      (Platform Abstraction Layer)       │
└─────────────────┬───────────────────────┘
                  │
         ┌────────┴────────┐
         │                 │
    ┌────▼────┐      ┌────▼────┐
    │ Android │      │   iOS   │
    │  Toast  │      │  Alert  │
    └─────────┘      └─────────┘
```

### Component Structure

```
com.weather.toast/
├── package.json                    # Package manifest
├── README.md                       # This file
├── Runtime/
│   ├── ToastController.cs         # Main controller class
│   ├── Plugins/
│   │   └── iOS/
│   │       └── NativeAlert.mm     # iOS native plugin
│   └── com.weather.toast.Runtime.asmdef  # Assembly definition
```

### Class Structure

```
┌──────────────────────────────────┐
│      ToastController             │
├──────────────────────────────────┤
│ + ShowToast(string) : void       │
│ + ShowToast(string, string)      │
│ - ShowNativeMessage(string)      │
│ - ShowAndroidToast(string)       │  // Android only
│ - ShowIOSAlert(string, string)   │  // iOS only
└──────────────────────────────────┘
```

### Platform Compilation

The package uses conditional compilation to ensure platform-specific code only compiles for target platforms:

```csharp
#if UNITY_ANDROID && !UNITY_EDITOR
    // Android-specific code
#elif UNITY_IOS && !UNITY_EDITOR
    // iOS-specific code
#else
    // Editor fallback (Debug.Log)
#endif
```

### Key Architectural Decisions

1. **Platform Abstraction**
   - Single public API (ShowToast methods)
   - Platform detection happens automatically
   - No platform-specific code in client code

2. **Namespace Isolation**
   - Uses WeatherToast namespace
   - Prevents naming conflicts
   - Clean integration

3. **Native Integration**
   - Android: JNI bridge to Java Toast class
   - iOS: DllImport to Objective-C++ plugin
   - No Unity UI dependencies

## Testing Framework

### Unity Test Framework

The package is designed to be testable using Unity Test Framework (NUnit-based).

#### Example Test Cases

```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WeatherToast;

public class ToastControllerTests
{
    [Test]
    public void ToastController_ShowToast_WithValidMessage_ShowsMessage()
    {
        GameObject testObject = new GameObject();
        WeatherToast.ToastController controller = testObject.AddComponent<WeatherToast.ToastController>();

        controller.ShowToast("Test message");

        LogAssert.Expect(LogType.Log, "[Toast] Test message");

        Object.Destroy(testObject);
    }

    [Test]
    public void ToastController_ShowToast_WithTitleAndMessage_ShowsMessage()
    {
        GameObject testObject = new GameObject();
        WeatherToast.ToastController controller = testObject.AddComponent<WeatherToast.ToastController>();

        controller.ShowToast("Test Title", "Test message");

        LogAssert.Expect(LogType.Log, "[Toast] Test message");

        Object.Destroy(testObject);
    }
}
```

#### Running Tests

1. Open Unity Editor
2. Go to Window → General → Test Runner
3. Select PlayMode or EditMode tab
4. Click Run All or select specific tests

## Platform-Specific Behavior

### Android
- **UI Element:** Toast (bottom of screen)
- **Duration:** LENGTH_LONG (approximately 3.5 seconds)
- **Dismissal:** Automatic
- **Threading:** Runs on UI thread via runOnUiThread()

### iOS
- **UI Element:** UIAlertController (center of screen)
- **Duration:** Until user taps OK
- **Dismissal:** Manual (user must tap button)
- **Threading:** Dispatched to main queue

### Unity Editor
- **Behavior:** Logs to Console
- **Format:** `[Toast] [message]`
- **Purpose:** Testing and debugging

## API Reference

### ToastController Class

#### Public Methods

| Method | Description | Returns |
|--------|-------------|---------|
| `ShowToast(string message)` | Shows a toast with the given message | void |
| `ShowToast(string title, string message)` | Shows a toast with title and message (iOS shows both, Android shows only message) | void |

#### Platform-Specific Methods (Internal)

| Method | Platform | Description |
|--------|----------|-------------|
| `ShowAndroidToast(string)` | Android | Shows native Android Toast |
| `ShowIOSAlert(string, string)` | iOS | Shows native iOS UIAlert |
| `ShowNativeMessage(string)` | All | Platform router |

## Requirements

- Unity 2019.4 or higher
- Android API Level 21+ (for Android builds)
- iOS 10.0+ (for iOS builds)

## Troubleshooting

### Android: Toast Not Showing
- Verify Android API level is 21 or higher
- Check logcat for Java exceptions
- Ensure app has UI permissions

### iOS: Alert Not Showing
- Verify iOS deployment target is 10.0+
- Check Xcode console for errors
- Ensure NativeAlert.mm is included in build

### Editor: No Output
- Check Console window for `[Toast]` messages
- Verify conditional compilation is working


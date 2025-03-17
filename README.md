# Coupon JS Injector

A local proxy server application that injects custom JavaScript into Amazon.com web pages. This tool intercepts HTTP responses from Amazon domains and injects specified JavaScript code before serving the content to your browser.


## Features

- Creates a local proxy server on port 8000
- Automatically configures system proxy settings
- Intercepts and modifies HTTP responses from Amazon domains
- Injects custom JavaScript into Amazon web pages
- Safely restores original proxy settings on exit
- Includes comprehensive error handling and cleanup
- 
## Technical Implementation

This application is built using:
- C# (.NET)
- [Titanium.Web.Proxy](https://github.com/justcoding121/Titanium-Web-Proxy) library for proxy functionality

- ## Current JavaScript Functionality

The current implementation injects JavaScript that:
1. Creates a popup notification in the center of the screen
2. Displays a customizable message
3. Provides a close button
4. Automatically closes after 10 seconds

### Usage

1. Run the application with administrator privileges:
   ```
   dotnet run
   ```
   Or launch the compiled executable with administrator rights

2. The application will:
   - Start a proxy server on port 8000
   - Configure your system to use this proxy
   - Begin intercepting and modifying Amazon web page responses

3. Browse to any Amazon website, and you should see the JavaScript injection in action (the popup will appear)

4. To stop the application:
   - Press any key in the console window
   - The application will automatically restore your original proxy settings
  
     
## How It Works

1. **Proxy Setup**: Creates a local proxy server and configures your system to use it
2. **Request Interception**: Monitors all HTTP traffic passing through the proxy
3. **Response Filtering**: Identifies responses from Amazon domains
4. **Content Modification**: For matching responses, injects custom JavaScript before the closing `</body>` tag
5. **Cleanup**: Properly restores original proxy settings when the application exits

## Safety Features

The application includes several safety mechanisms:

- **Comprehensive cleanup**: Restores original proxy settings even if the application crashes
- **Multiple exit handlers**: Handles console cancellation, process exit, and unhandled exceptions
- **Status logging**: Maintains a log file (`proxy_status.txt`) to track proxy state
- **IDisposable implementation**: Ensures proper cleanup when used in Visual Studio debugger

## Troubleshooting

### Proxy Not Starting

- Ensure you're running the application with administrator/root privileges
- Check if another application is using port 8000
- Look for error messages in the console output

### JavaScript Not Injecting

- Verify you're visiting an Amazon domain
- Check that the response is HTML (JavaScript won't be injected into JSON/API responses)
- Inspect the `proxy_status.txt` file to confirm the proxy is active

### Unable to Restore Proxy Settings

If the application fails to restore your original proxy settings:

**Windows**:
1. Open Internet Options (via Control Panel)
2. Go to Connections > LAN settings
3. Uncheck "Use a proxy server for your LAN"

**macOS**:
1. Open System Preferences > Network
2. Select your active network connection
3. Click Advanced > Proxies
4. Uncheck any enabled proxy settings

**Linux** (Ubuntu/Debian):
1. Open System Settings > Network
2. Click on the gear icon for your connection
3. Go to Proxy settings and disable

## Advanced Usage

### Customizing Injected JavaScript

The JavaScript can be modified to perform various actions on Amazon pages:
- Add custom UI elements
- Modify page content or styling
- Add additional functionality
- Extract information (use caution with sensitive data)

### Targeting Specific Pages

To target only specific Amazon pages, modify the URL check in the `OnResponse` method:

```csharp
if (e.HttpClient.Request.Url.Contains("amazon.com/product") && ...)
```

## Properly Closing the Application

To ensure proper cleanup and restoration of your system's proxy settings:

1. **Recommended Method**: Press any key in the console window
   - This triggers the normal shutdown sequence
   - The application will log "inactive" to the status file
   - Your original proxy settings will be restored

2. **Alternative Methods**:
   - Press Ctrl+C in the console
   - Close the console window
   - End the process via Task Manager/Activity Monitor

3. **Verification**: Check the `proxy_status.txt` file - it should contain the text "inactive" when properly closed

If the application doesn't close properly:
1. Run it again
2. Allow it to start
3. Close it properly using one of the above methods

namespace CouponInjector
{
    public class AmazonJavaScriptInjector : IJavaScriptInjector
    {
        private readonly string _javascriptCode;

        public AmazonJavaScriptInjector()
        {
            _javascriptCode = @"
            // Create a div element for the popup
            const popup = document.createElement('div');
            // Style the popup
            popup.style.position = 'fixed';
            popup.style.top = '50%';
            popup.style.left = '50%';
            popup.style.transform = 'translate(-50%, -50%)';
            popup.style.backgroundColor = 'white';
            popup.style.padding = '20px';
            popup.style.borderRadius = '5px';
            popup.style.boxShadow = '0 0 10px rgba(0, 0, 0, 0.3)';
            popup.style.zIndex = '9999';
            popup.style.maxWidth = '300px';
            popup.style.textAlign = 'center';
            // Add content to the popup
            popup.innerHTML = `
                <h3 style=""margin-top: 0; color: #232f3e;"">Hello, this is JS</h3>
                <p>This popup is injected via JavaScript</p>
                <button id=""closePopup"" style=""background-color: #ff9900; color: white; border: none; padding: 8px 15px; border-radius: 3px; cursor: pointer;"">Close</button>
            `;
            // Append the popup to the body
            document.body.appendChild(popup);
            // Add event listener to the close button
            document.getElementById('closePopup').addEventListener('click', function() {
                document.body.removeChild(popup);
            });
            // Auto-close the popup after 10 seconds
            setTimeout(function() {
                if (document.body.contains(popup)) {
                    document.body.removeChild(popup);
                }
            }, 10000);";
        }

        public string InjectJavaScript(string html)
        {
            return html.Replace("</body>", $"<script>{_javascriptCode}</script></body>");
        }
    }
}

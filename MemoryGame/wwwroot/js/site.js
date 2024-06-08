// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/** sendAsyncRequest
 * 
 * send a async POST request with JSON content to an specific endpoint, if the status code == 200, alert the succes message
 * 
 */
async function sendAsyncRequest(endpoint, data, requestMethod, succesMessage) {
    try {
        const response = await fetch(endpoint, {
            method: requestMethod,
            headers: {
                'Content-Type': 'application/json',
            },
            body: data,
        });

        if (!response.ok) {
            throw new Error(`HTTP error, Status: ${response.status}`);
        }
        if (succesMessage != null){
            window.alert(succesMessage);
        }
        return await response.status;
    } catch (error) {
        console.error('Error:', error);
    }
}
"use strict";
var access_token = 'eyJhbGciOiJSUzI1NiIsImtpZCI6IkVCQjAxQkZEQjcxOUNGODAxOUMzREU4REI1RTNEQTJBIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2NjY0Mjk1NDQsImV4cCI6MTY2NjUxNTk0NCwiaXNzIjoiaHR0cHM6Ly9pZGVudGl0eXN0cy5heG9ucy5kZXYiLCJhdWQiOiJheG9uX2lkZW50aXR5X2FkbWluX2FwaSIsImNsaWVudF9pZCI6ImNwb2UtZnJvbnQiLCJzdWIiOiI1MjMiLCJhdXRoX3RpbWUiOjE2NjY0Mjk1NDQsImlkcCI6ImxvY2FsIiwianRpIjoiNDMwMDI4MzdDMkZEQzVCRTdENDY1ODQ0RjI5NjQ3NEMiLCJpYXQiOjE2NjY0Mjk1NDQsInNjb3BlIjpbImF4b25faWRlbnRpdHlfYWRtaW5fYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.sqCHFs3PIQrLSDACnhdHOAuJ6pUOvlOJPwNdDCDlrhf5qYCWu2XZ97aXleCH7vButEYDQ5A7AbRmWF7CGTcwt-HfcdNRZoYK33t_F0tK3U5F54-j6jel3zmV3tF4egBlDI-9jxc_s6QGxgbc48L2L1UAjK_68shWLFzqKDUaOka0MjLYg81sx70eGLibhPuIWFxBKDK4J_E__2BI1lOV3WlIle0R0yF-NyiZyMApzzSDCgtjgUvhAWzpX84TIOMZ6DPJek4IdUbJrCcWtW4U8SDbKuy2hCQwn8CT2wp3uvFS24GhYiAl0mwH4iYx8d0UolnY8wTMSHDLt0fM4w6i6A';

function getQuery(prop) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(prop);
}

var connection = new signalR.HubConnectionBuilder().withUrl('http://10.30.72.4:8001/RealTimeHub', {
//var connection = new signalR.HubConnectionBuilder().withUrl('https://localhost:44301/RealTimeHub', {
    accessTokenFactory: () => {
        return getQuery('jwt');
        // Get and return the access token.
        // This function can return a JavaScript Promise if asynchronous
        // logic is required to retrieve the access token.
    }
}).build();


/****************************************************************************
* Signaling server
****************************************************************************/
connection.onclose(function () {
    console.log('Disconnected From SignalR');

    //setTimeout(function () {
    //    connection.start().then(onsignalRConnect).catch(onsignalRCatch);
    //}, 1000);
});
// Connect to the signaling server

function onsignalRConnect() {
    console.log('connected From SignalR, Connection Id :', connection);
    connection.on('message', function (type,message) {
        console.log('Get From Server:',type, message);
    });
    
}
function onsignalRCatch(err) {
    console.log('err in signalR : ', err);
    //setTimeout(function () {
    //    connection.start().then(onsignalRConnect).catch(onsignalRCatch);
    //}, 1000);
    return console.error(err.toString());
}

connection.start().then(onsignalRConnect).catch(onsignalRCatch);



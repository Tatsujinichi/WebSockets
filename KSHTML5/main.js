//function allowDrop(ev) {
//    ev.preventDefault();
//}
//
//function drag(ev) {
//    ev.dataTransfer.setData("text/html", ev.target.id);
//}
//
//function drop(ev) {
//    ev.preventDefault();
//    var data = ev.dataTransfer.getData("text/html");
//    ev.target.appendChild(document.getElementById(data));
//}

function Connect(ev){
    CreateChatClient();
}

function Send(ev){
    var inputArea = document.getElementById("inputTextArea");
    var textToSend = inputArea.text;
    window.client.Send(textToSend);
}

function CreateChatClient()
{
    wsUri = "ws://127.0.0.1:555";
    window.client = new ChatClient(wsUri);
}

function ChatClient(ip)
{
    this._wsUri = ip;
    this._client = new WebSocket(this._wsUri);
    this._client.onopen = function(evt) { onOpen(evt) };
    this._client.onclose = function(evt) { onClose(evt) };
    this._client.onmessage = function(evt) { onMessage(evt) };
    this._client.onerror = function(evt) { onError(evt) };
}

ChatClient.prototype.Send = function(message)
{
    this._client.send(message);
};

function onOpen(evt)
{
    var textArea = document.getElementById("outputTextArea");
    textArea.value = "onOpen";
    window.client.Send("HTML5 WebSocket rocks!/n");
}

function onClose(evt)
{
    var textArea = document.getElementById("outputTextArea");
    textArea.value = "onClose";
}

function onMessage(evt)
{
    var textArea = document.getElementById("outputTextArea");
    textArea.value = "onMessage";
}

function onError(evt)
{
    var textArea = document.getElementById("outputTextArea");
    textArea.value = "onError";
}



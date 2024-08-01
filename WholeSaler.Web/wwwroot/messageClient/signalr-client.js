$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/messagehub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    const receiveMessage = "receiveMessage";
    const receiveConnectedClientNumber = "receiveConnectedClientNumber";
    const receiveMessageForPrivateClient = "receiveMessageForPrivateClient";
    const receiveMessageForOthersClient = "receiveMessageForOthersClient";
    const receiveMessageForSelectedClient = "receiveMessageForSelectedClient";
    const receiveMessageForGroupClients = "receiveMessageForGroupClients";
    const receiveTypedMessage = "receiveTypedMessage";

    const groupA = "Group-A";
    const groupB = "Group-B";
    let currentGroupList = [];

    function refreshGroupList() {
        $("#groupList").empty();
        currentGroupList.forEach(x => {
            $("#groupList").append(`<p>${x}</p>`);
        });
    }

    $("#btn-join-groupA").click(function () {
        if (currentGroupList.includes(groupA)) return;
        connection.invoke("JoinToGroup", groupA).then(() => {
            currentGroupList.push(groupA);
            refreshGroupList();
        }).catch(err => console.error(err));
    });

    $("#btn-leave-groupA").click(function () {
        if (!currentGroupList.includes(groupA)) return;
        connection.invoke("LeaveFromGroup", groupA).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupA);
            refreshGroupList();
        }).catch(err => console.error(err));
    });

    $("#btn-join-groupB").click(function () {
        if (currentGroupList.includes(groupB)) return;
        connection.invoke("JoinToGroup", groupB).then(() => {
            currentGroupList.push(groupB);
            refreshGroupList();
        }).catch(err => console.error(err));
    });

    $("#btn-leave-groupB").click(function () {
        if (!currentGroupList.includes(groupB)) return;
        connection.invoke("LeaveFromGroup", groupB).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupB);
            refreshGroupList();
        }).catch(err => console.error(err));
    });

    $("#btn-message-groupA").click(function () {
        const message = "Welcome to Group-A";
        connection.invoke("BroadcastGroupClientMessage", groupA, message).catch(err => console.error(err));
        console.log("Message sent to Group-A");
    });

    $("#btn-message-groupB").click(function () {
        const message = "Welcome to Group-B";
        connection.invoke("BroadcastGroupClientMessage", groupB, message).catch(err => console.error(err));
        console.log("Message sent to Group-B");
    });

    $("#btn-typed-message").click(async function () {
        var header = document.getElementById("message-header").value;
        var body = document.getElementById("message-body").value;
        var newMessage = { header: header, body: body };

        connection.invoke("BroadcastTypedMessage", newMessage).catch(err => console.error(err));
        console.log("Typed message sent");
    });

    function start() {
        connection.start().then(() => {
            console.log("Hub connection established");
            $("#div-connection-id").html(`Connection Client Id : ${connection.connectionId}`);
        }).catch(err => {
            console.error(err.toString());
            setTimeout(start, 5000);
        });
    }

    start();

    connection.on(receiveMessage, (message) => {
        console.log("Received Message: ", message);
    });

    const span_client_number = $("#span-connected-client-number");

    connection.on(receiveConnectedClientNumber, (count) => {
        span_client_number.text(`(${count})`);
        console.log("Connected Client Number: ", count);
    });

    connection.on(receiveMessageForPrivateClient, (message) => {
        console.log("(Private) Message: ", message);
    });

    connection.on(receiveMessageForOthersClient, (message) => {
        console.log("(Others) Message: ", message);
    });

    connection.on(receiveMessageForSelectedClient, (message) => {
        console.log("(Selected Client) Message: ", message);
    });

    connection.on(receiveMessageForGroupClients, (message) => {
        console.log("Group Message: ", message);
    });

    connection.on(receiveTypedMessage, (message) => {
        console.log("Typed Message: ", message);
    });

    $("#btn-send-message").click(function () {
        const message = "Welcome to the project";
        connection.invoke("BroadcastMessage", message).catch(err => console.error(err));
    });

    $("#btn-send-message-for-private-client").click(function () {
        const message = "Welcome to the project - private client -";
        connection.invoke("BroadcastMessageForPrivateClient", message).catch(err => console.error(err));
    });

    $("#btn-send-message-for-others-client").click(function () {
        const message = "Welcome to the project - others client -";
        connection.invoke("BroadcastMessageForOthersClient", message).catch(err => console.error(err));
    });

    $("#btn-send-message-for-selected-client").click(function () {
        const message = $('#private-message').val();
        const connectionId = $("#text-connectionId").val();
        connection.invoke("BroadcastMessageForSelectedClient", connectionId, message).catch(err => console.error(err));
    });
});

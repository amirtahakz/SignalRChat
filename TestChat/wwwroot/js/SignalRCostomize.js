$(document).ready(function () {
    if (Notification.permission !== "granted") {
        Notification.requestPermission();
    }
});
var currentGroupId = 0;
var userId = 0;

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

connection.on("Welcome",
    function (id) {
        userId = id;
    });

connection.on("ReceiveMessage", receive);
connection.on("NewGroup", appendGroup);
connection.on("JoinGroup", joined);
connection.on("ReceiveNotification", sendNotification);
connection.on("Test",
    function (first) {
        console.log(first);
    });

connection.start();

//invoke = فراخوانی کردن

function sendNotification(chat) {
    if (Notification.permission === "granted") {
        console.log(chat.groupId);
        console.log(currentGroupId);
        if (currentGroupId !== chat.groupId) {
            var notification = new Notification(chat.groupName,
                {
                    body: chat.chatBody
                });

        }
    }
}

function receive(chat) {
    $("#messageText").val('');
    if (userId === chat.userId) {
        $(".chats").append(`
              <div class="row no-gutters">
                <div class="col-md-3">
                  <div class="chat-bubble chat-bubble--left">
                    ${chat.chatBody}
                   <span>${chat.createDate}</span>
                  </div>
                </div>
              </div>
                `);
    } else {
        $(".chats").append(`
              <div class="row no-gutters">
                <div class="col-md-3 offset-md-9">
                  <div class="chat-bubble chat-bubble--right">
                    ${chat.chatBody}
                   <span>${chat.createDate}</span>
                     ${chat.userName}
                  </div>
                </div>
              </div>
                `);
    }

}

function SendMessage(event) {
    event.preventDefault();
    var text = $("#messageText").val();
    if (text) {
        connection.invoke("SendMessage", text, currentGroupId);

    } else {
        alert("Error");
    }
}

function appendGroup(groupName, token, imageName) {
    if (groupName === "Error") {
        alert("ERROR");
    } else {
        $("#user_groups").append(`
                   <div class="friend-drawer friend-drawer--onhover" onclick="joinInGroup('${token}')">
                                     <img class="profile-image" src="/image/groups/${imageName}" alt="">
                                 <div class="text">
                                     <h6>${groupName}</h6>
                                     <p class="text-muted">Hey, you're arrested!</p>
                                 </div>
                    </div>
                                                `);
        $("#exampleModal").modal({ show: false });
    }
}

function insertGroup(event) {
    event.preventDefault();
    var groupName = event.target[1].value;
    var imageFile = event.target[2].files[0];
    var formData = new FormData();
    formData.append("GroupName", groupName);
    formData.append("ImageFile", imageFile);

    $.ajax({
        url: "/home/CreateGroup",
        type: "post",
        data: formData,
        dataType: 'json',
        encytype: "multipart/form-data",
        processData: false,
        contentType: false
    });
}

function search() {
    var text = $("#search_input").val();
    if (text) {
        $("#search_result").show();
        $("#user_groups").hide();
        $.ajax({
            url: "/home/search?title=" + text,
            type: "get"
        }).done(function (data) {
            $("#search_result").html("");
            for (var i in data) {
                if (data[i].isUser) {
                    //$("#search_result").append(`
                    //             <li onclick="joinInPrivateGroup(${data[i].token})">
                    //                        ${data[i].title}
                    //                        <img src="/img/${data[i].imageName}" class="profile-image"/>
                    //                        <span></span>
                    //                    </li>
                    //    `);
                    $("#search_result").append(`
                                 <div class="friend-drawer friend-drawer--onhover" onclick="joinInPrivateGroup(${data[i].token}))">
                                     <img class="profile-image" src="/img/${data[i].imageName}" alt="">
                                 <div class="text">
                                     <h6>${data[i].title}</h6>
                                     <p class="text-muted">Hey, you're arrested!</p>
                                 </div>
                    </div>
                        `);
                } else {
                    $("#search_result").append(`
                                 <div class="friend-drawer friend-drawer--onhover" onclick="joinInGroup('${data[i].token}')">
                                     <img class="profile-image" src="/image/groups/${data[i].imageName}" alt="">
                                 <div class="text">
                                     <h6>${data[i].title}</h6>
                                     <p class="text-muted">Hey, you're arrested!</p>
                                 </div>
                    </div>
                        `);
                }
            }

        });
    } else {
        $("#search_result").hide();
        $("#user_groups").show();
    }
}

function joined(group, chats) {
    $(".header").css("display", "block");
    $("#footer").css("display", "block");
    $(".header h6").html(group.groupTitle);
    $(".header img").attr("src", `/image/groups/${group.imageName}`);
    currentGroupId = group.id;
    $(".chats").html("");
    for (var i in chats) {
        var chat = chats[i];
        if (userId === chat.userId) {
            $(".chats").append(`
              <div class="row no-gutters">
                <div class="col-md-3">
                  <div class="chat-bubble chat-bubble--left">
                    ${chat.chatBody}
                   <span>${chat.createDate}</span>
                  </div>
                </div>
              </div>
                `);
        } else {
            $(".chats").append(`
              <div class="row no-gutters">
                <div class="col-md-3 offset-md-9">
                  <div class="chat-bubble chat-bubble--right">
                    ${chat.chatBody}
                   <span>${chat.createDate}</span>
                     ${chat.userName}
                  </div>
                </div>
              </div>
                `);
        }
    }
}

function joinInGroup(token) {
    connection.invoke("JoinGroup", token, currentGroupId);
}

function joinInPrivateGroup(receiverId) {
    connection.invoke("JoinPrivateGroup", receiverId, currentGroupId);
}

//function SendMessage(event) {
//    event.preventDefault();
//    var text = $("#messageText").val();
//    connection.invoke("SendMessage", text);
//}

//function insertGroup(event) {
//    event.preventDefault();
//    var text = $("#groupName").val();
//    if (text) {
//        connection.invoke("CreateGroup", text);
//    }
//}


//async function Start() {
//    try {
//        await connection.start();
//        $(".disConnect").show();

//    } catch (e) {
//        $(".disConnect").hide();
        
//        setTimeout(Start, 6000);
//    }
//}

//connection.onclose(Start);
//Start();
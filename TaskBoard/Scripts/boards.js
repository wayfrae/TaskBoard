//set signalR

var boardIdForRemoval = "";

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

//hide some elements on load
$(document).ready(function () {
    $("span.fa-lock").hide();
    $("#groupForm").hide();
});

$("span.fa-unlock").click(function () {
	$(this).hide();
	$(this).siblings("span.fa-lock").show();
});

$("span.fa-lock").click(function () {
	$(this).hide();
	$(this).siblings("span.fa-unlock").show();
});

$("#groupFormButton").click(function () {

});

function getJSON(type) {
    var obj = [];
    var id, title, body, locked, owner;

    if (type == "addBoard") {
        var selection = $("#groupSelection");
        var groupId = selection.children[selection.selectedIndex].value;
        obj.push({ type: type, groupId: groupId });
    } else if (type == "addGroup") {
        var name = $("#groupForm").children("input:first").val();
        obj.push({ type: type, name: name });
    } else if (type == "removeBoard") {
        obj.push({ type: type, boardId: boardIdForRemoval });
    } else {
        obj.push({ type: type });
    }

    var count = 0;
    $('#boardContainer').children('div.card:not(#newBoard)').each(function () {
        console.log($(this));
        id = $(this).attr('boardID');
        owner = $('#groupSelection').find(":selected").val();
        $(this).children('div').each(function () {

            $(this).children('span').each(function () {
                if ($(this).hasClass('card-title')) {
                    title = $(this).text();
                }
                if ($(this).hasClass('fa-unlock') && $(this).is(':visible')) {
                    locked = 0;
                }
                if ($(this).hasClass('fa-lock') && $(this).is(':visible')) {
                    locked = 1;
                }
            });

            if ($(this).hasClass('card-body')) {
                body = $(this).children('p').text();
            }
        });   
        obj.push({ id: id, title: title, body: body, locked: locked, owner: owner });
        count++;
    });
    //console.log(obj);
    return JSON.stringify(obj);
}


$(function () {
    // Declare a proxy to reference the hub.     
    var server = $.connection.boardHub;

    server.client.broadcast = function (message) {
        // Add the message to the page.
        console.log(message);
        var obj = JSON.parse(message);
        //console.log(obj);
        updateAllBoards(obj);
    };

    $.connection.hub.start().done(function () {
        $('div.card').keyup(function () {
            // Call the Send method on the hub.
            server.server.send(getJSON("normal"));
        });

        $("#newBoardIcon").click(function () {
            server.server.send(getJSON("addBoard"));
        });

        $("#groupFormButton").click(function () {
            server.server.send(getJSON("addGroup"));
        });

        $(".fa-trash-alt").click(function () {
            boardIdForRemoval = this.parentNode.parentNode.boardid;
            server.server.send(getJSON("removeBoard"));
        });
    });
});

//animate new board button
$("#newBoard").mousedown(function () {
    $(this).removeClass("shadow-lg");
    $(this).css("margin-top", "87px");
});
$("#newBoard").mouseup(function () {
    $(this).addClass("shadow-lg");
    $(this).css("margin-top", "85px");
});

//animate group form
$("#addGroupButton").click(function () {
    $("#groupForm").toggle("slide", { direction: "left"}, 500);
});


//update all the boards
function updateAllBoards(obj) {
    for (var i = 0; i < obj.length; i++) {
        console.log("ObjectID: " + obj[i].id);
        console.log($("#boardContainer").find("div.card[boardID=" + obj[i].id + "]"));
        $("#boardContainer").find("div.card[boardID=" + obj[i].id + "]").children('div').each(function () {
            
            $(this).children('span').each(function () {
                if ($(this).is(":focus")) {
                    return false;
                }
                if ($(this).hasClass('card-title')) {
                    $(this).text(obj[i].title);
                }

                if (obj[i].locked == 0) {
                    if ($(this).hasClass('fa-lock')) {
                        $(this).hide();
                    }
                    if ($(this).hasClass('fa-unlock')) {
                        $(this).show();
                    }
                } else {
                    if ($(this).hasClass('fa-lock')) {
                        $(this).show();
                    }
                    if ($(this).hasClass('fa-unlock')) {
                        $(this).hide();
                    }
                }                
            });

            if ($(this).hasClass('card-body')) {
                if ($(this).children('p').is(":focus")) {
                    return false;
                }
                $(this).children('p').text(obj[i].body);
            }
        });        
    }
}
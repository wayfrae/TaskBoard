﻿//set signalR

var boardIdForRemoval = "";
var lockedID = 0;
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

//hide some elements on load
$(document).ready(function () {
    $("span.fa-lock[data-locked=False]").hide();
    $("span.fa-unlock[data-locked=True]").hide();
    $("#groupForm").hide();
});



function getJSON(type) {
    var obj = [];
    var id, title, body, locked, owner;

    if (type == "addBoard") {
        var groupId = $("#groupSelection").find(":selected").val();
        //var groupId = selection.children[selection.selectedIndex].value;
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
        id = $(this).attr('boardID');
        owner = $('#groupSelection').find(":selected").val();
        $(this).children('div').each(function () {

            $(this).children('span').each(function () {
                if ($(this).hasClass('card-title')) {
                    title = $(this).text();
                }               
                console.log($(this).attr("data-locked") == "True");
                if ($(this).has("attr", "data-locked") && $(this).attr("data-locked") == "True") {
                    locked = 1;
                } else {
                    locked = 0;
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

$('#groupSelection').change(function () {
    //console.log($("#boardContainer").children('div.card').has('attr', 'groupID'));
    $("#boardContainer").children('div.card.shadow').each(function () {
        $(this).addClass('d-none');
    });
    $("#boardContainer").children('div.card[groupID=' + $('#groupSelection').val() + ']').toggleClass('d-none');

});

$(function () {
    // Declare a proxy to reference the hub.     
    var server = $.connection.boardHub;

    server.client.broadcast = function (message) {
        // Add the message to the page.
        console.log(message);
        var obj = JSON.parse(message);
        //if json object is for group
        if (typeof (obj[0].Name) !== 'undefined') {
            $('#groupSelection').find('option').remove().end();
            for (var i = 0; i < obj.length; i++) {
                $('#groupSelection').append(new Option(obj[i].Name, obj[i].ID));
            }
        } else {
            console.log(obj);
            updateAllBoards(obj);
        }
        
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
            $("#groupInput").val("");
            $("#groupForm").toggle("slide", { direction: "left" }, 250);
        });

        $(".fa-trash-alt").click(function () {
            boardIdForRemoval = $(this).parent().parent().attr("boardID");
            //console.log(boardIdForRemoval);
            server.server.send(getJSON("removeBoard"));
        });

        $("span.fa-unlock").click(function () {
            $(this).hide();
            $(this).siblings("span.fa-lock").attr("data-locked","True").show();
            server.server.send(getJSON("normal"));
            lockedID = $(this).parent().parent().attr("boardID");
        });

        $("span.fa-lock").click(function () {

            if (lockedID == $(this).parent().parent().attr("boardID")) {
                $(this).hide();
                $(this).siblings("span.fa-unlock").attr("data-locked", "False" ).show();
                server.server.send(getJSON("normal"));
            }

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

function addBoardToPage(obj) {    
    $("#newBoard").before(`<div class="card shadow" boardID="` + obj.ID + `" groupID="` + obj.Owner + `">
            <div class="card-header">
                <span class="h4 card-title" contenteditable="true">&nbsp;</span>
                <span name="lockIcon"
                      data-toggle="tooltip"
                      data-locked="True"
                      data-placement="top"
                      title="Click to unlock" class="fa fa-2x fa-lock pull-right hidden"></span>
                <span name="unlockIcon"
                      data-locked="True"
                      data-toggle="tooltip"
                      data-placement="top"
                      title="Click to lock"
                      class="fa fa-2x fa-unlock pull-right"></span>
            </div>
            <div class="card-body">
                <p contenteditable="true" class="card-text">&nbsp;</p>
                <span data-toggle="tooltip"
                      data-placement="top"
                      title="Delete board"
                      class="fa fa-2x fa-trash-alt text-danger pull-right">
                </span>
            </div>
        </div>`);
    lockedID = obj.ID;
}

function removeBoardFromPage() {
    $("#boardContainer").find("div.card[boardID=" + boardIdForRemoval + "]").remove();
}

//update all the boards
function updateAllBoards(obj) {
    var visibleBoards = $("#boardContainer").children('div').length - 1;
    for (var i = 0; i < obj.length; i++) {
        //console.log("Object length: " + obj.length);
        //console.log("Visible boards length: " + visibleBoards);
        //console.log($("#boardContainer").find("div.card[boardID=" + obj[i].id + "]"));
        if (visibleBoards < obj.length) {
            addBoardToPage(obj[obj.length - 1]);
            break;
        }else if (visibleBoards > obj.length) {
            removeBoardFromPage();
            break;
        } else {
            $("#boardContainer").find("div.card[boardID=" + obj[i].ID + "]").children('div').each(function () {
                var lockedP;
                $(this).children('span').each(function () {
                    if ($(this).is(":focus")) {
                        return false;
                    }
                    if ($(this).hasClass('card-title')) {
                        $(this).text(obj[i].Title);

                    }
                    //console.log(obj[i].IsLocked);
                    if (!obj[i].IsLocked) {
                        lockedP = 0;
                        if ($(this).hasClass('fa-lock')) {
                            $(this).hide();
                        }
                        if ($(this).hasClass('fa-unlock')) {
                            $(this).show();
                        }
                    } else {
                        lockedP = 1;
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
                    $(this).children('p').text(obj[i].Body);


                    if ($(this).hasClass('card-body')) {
                        $(this).children('p').each(function () {
                            var lockNum = $(this).parent().parent().attr("boardID");
                            if (lockedP == 1 && lockNum != lockedID) {
                                $(this).attr('contenteditable', 'false');
                                $(this).parent().siblings('.card-title').attr("contenteditable", "false");
                            } else if (lockedP == 1 && lockNum == lockedID) {
                                $(this).parent().siblings('.card-header').children('.card-title').attr("contenteditable", "true");
                                $(this).attr('contenteditable', 'true');
                            }else {
                                $(this).parent().siblings('.card-header').children('.card-title').attr("contenteditable", "false");
                                $(this).attr('contenteditable', 'false');
                            }
                        })
                    }
                }
            }); 
        }               
    }
}
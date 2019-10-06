<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jQuery_Sortable_From_Database.aspx.cs" Inherits="jQuery_Sortable_From_Database.jQuery_Sortable_From_Database" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">
        var dataBackup;
        $(document).ready(function () {

            $('#answerOption').sortable({
                animation: 150,
                placeholder: '.placeholder',
                cursor: "move",
                axis: 'y',
                zIndex: 9999,
                start: function () {
                    $('#answerOption li').removeClass('wrongAnswer correctAnswer');
                }
            });
            $.ajax({
                url: 'Services/QuestionService.asmx/GetQuestionData',
                method: 'post',
                dataType: 'json',
                success: function (data) {
                    $('#question').text(data.QuestionText);

                    $(data.QuestionOptions).each(function () {
                        $('#questionOptions').append('<li id =' + this.Id + ' class="">' + this.OptionText + '</li>');
                    });
                    dataBackup = data.AnswerOptions;
                    $(data.AnswerOptions).each(function () {
                        $('#answerOption').append('<li id =' + this.Id + ' class="box">' + this.OptionText + '</li>');
                    });

                    $('#answerOption').sortable({
                        placeholder: '.placeholder',
                        axis: 'y',
                        start: function () {
                            $('#answerOption li').removeClass('wrongAnswer correctAnswer');
                        }
                    });
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });

            $("#btnReset").on('click', function () {
                //debugger;
                //$('#answerOption li').remove()
                //$(dataBackup).each(function () {
                //        $('#answerOption').append('<li id =' + this.Id + '>' + this.OptionText + '</li>');
                //    });

                var ul = $("#answerOption");
                var li;
                li = $('#answerOption li').detach().sort(function (a, b) {
                    return a.id - b.id;
                });
                ul.append(li).show('slow');
                ul.animate({ left: '250px' });

            })

            $('#btnCheck').click(function () {
                $.ajax({
                    url: 'Services/QuestionService.asmx/GetAnswerData',
                    method: 'post',
                    dataType: 'json',
                    success: function (data) {
                        var userAnswer = '';

                        $('#answerOption li').each(function () {
                            userAnswer += $(this).attr('id') + ',';
                        });

                        userAnswer = userAnswer.substr(0, userAnswer.lastIndexOf(','));

                        if (userAnswer == data.AnswerText) {
                            $('#answerOption li').removeClass('wrongAnswer').addClass('correctAnswer');
                        }
                        else {
                            $('#answerOption li').removeClass('correctAnswer').addClass('wrongAnswer');
                        }
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            })

        });
    </script>
    <style>
        li {
            border: 1px solid black;
            padding: 10px;
            height: 20px;
            cursor: pointer;
            width: 150px;
            margin: 3px;
            color: black;
            list-style-type: none;
        }

        .placeholder {
            border: 1px solid black;
            padding: 10px;
            height: 20px;
            width: 150px;
            margin: 3px;
            color: black;
            background-color: ghostwhite;
            border-radius: 3px;
        }

        ul {
            float: left;
        }

        .ui-sortable-handle {
            background-color: #2f2f33;
            color: white;
            border-radius: 3px;
            text-align: center;
        }

        .correctAnswer {
            background-color: #5B84B1FF;
            color: white;
        }

        .wrongAnswer {
            background-color: #FC766AFF;
            color: white;
        }

        #answerOption li {
            background-color: #2f2f33;
            color: white;
            border-radius: 3px;
            text-align: center;
        }

        @keyframes expand {
            from {
                opacity: 0;
                background: #5470B0;
            }
        }

        .box {
            animation: expand .5s ease-in-out;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="question"></div>

            <ul id="questionOptions"></ul>

            <ul id="answerOption">
            </ul>

            <input id="btnCheck" type="button" value="Check Answer" style="clear: both; float: left;" />
            <input id="btnReset" type="button" value="Reset" style="clear: both; float: left;" />
        </div>
    </form>
</body>
</html>

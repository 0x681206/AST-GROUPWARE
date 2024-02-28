/*
Template Name: Minton - Admin & Dashboard Template
Author: CoderThemes
Website: https://coderthemes.com/
Contact: support@coderthemes.com
File: Calendar init js
*/

!function ($) {
    "use strict";

    var CalendarApp = function () {
        this.$body = $("body"),
        this.$modal = $('#event-modal'),
        this.$calendar = $('#calendar'),
        this.$formEvent = $("#form-event"),
        this.$btnNewEvent = $("#btn-new-event"),
        this.$btnDeleteEvent = $("#btn-delete-event"),
        this.$btnSaveEvent = $("#btn-save-event"),
        this.$modalTitle = $("#modal-title"),
        this.$modalTitle = $("#modal-title"),
        this.$allDay = $("#allDay"),
        this.$startHour = $("#startHour"),
        this.$startMinute = $("#startMinute"),
        this.$endHour = $("#endHour"),
        this.$endMinute = $("#endMinute"),
        this.$calendarObj = null,
        this.$selectedEvent = null,
        this.$newEventData = null
    };

    /* on click on event */
    CalendarApp.prototype.onEventClick = function (info) {
        this.$formEvent[0].reset();
        this.$formEvent.removeClass("was-validated");

        this.$newEventData = null;
        this.$btnDeleteEvent.show();
        this.$modalTitle.text('Edit Event');
        this.$modal.modal({
            backdrop: 'static'
        });
        this.$selectedEvent = info.event;

        const startDatetime = new Date(info.event.start);
        const endDatetime = new Date(info.event.end);
        this.$allDay.prop('checked', info.event.allDay);
        this.$startHour.val(startDatetime.getHours().toString());
        this.$startMinute.val(startDatetime.getMinutes().toString());
        this.$endHour.val(endDatetime.getHours().toString());
        this.$endMinute.val(endDatetime.getMinutes().toString());
        var days = ['日', '月', '火', '水', '木', '金', '土'];
        $('#modal-date').html(startDatetime.getFullYear() + "年" + (startDatetime.getMonth() + 1) + "月" + startDatetime.getDate() + "日（" + days[startDatetime.getDay()] + "）");
        $("#event-title").val(this.$selectedEvent.title);
        $("#event-memo").val(this.$selectedEvent.memo);
        var schedule_type = 0;
        switch (this.$selectedEvent.classNames[0]) {
            case 'bg-success':
                schedule_type = 1;
                break;
            case 'bg-info':
                schedule_type = 2;
                break;
            case 'bg-warning':
                schedule_type = 3;
                break;
            case 'bg-purple':
                schedule_type = 4;
                break;
            case 'bg-danger':
                schedule_type = 5;
                break;
            default:
                schedule_type = 0;
                break;
        }
        $("#event-category").val(schedule_type);
    },

    /* on select */
    CalendarApp.prototype.onSelect = function (info) {
        this.$formEvent[0].reset();
        this.$formEvent.removeClass("was-validated");

        this.$selectedEvent = null;
        this.$newEventData = info;

        const startDatetime = new Date(info.date);
        this.$allDay.prop('checked', info.allDay);
        startHour.value = (startDatetime.getHours()+8).toString();
        startMinute.value = startDatetime.getMinutes().toString();
        endHour.value = (startDatetime.getHours()+9).toString();
        endMinute.value = startDatetime.getMinutes().toString();
        
        this.$btnDeleteEvent.hide();
        this.$modalTitle.text('Add New Event');
        var days = ['日', '月', '火', '水', '木', '金', '土'];
        $('#modal-date').html(startDatetime.getFullYear() + "年" + (startDatetime.getMonth() + 1) + "月" + startDatetime.getDate() + "日（" + days[startDatetime.getDay()] + "）");

        this.$modal.modal({
            backdrop: 'static'
        });
        this.$calendarObj.unselect();
    },

    /* Initializing */
    CalendarApp.prototype.init = async function (defaultEvents) {

        /*  Initialize the calendar  */
        var today = new Date($.now());

        var Draggable = FullCalendarInteraction.Draggable;
        var externalEventContainerEl = document.getElementById('external-events');

        // init dragable
        new Draggable(externalEventContainerEl, {
            itemSelector: '.external-event',
            eventData: function (eventEl) {
                return {
                    title: eventEl.innerText,
                    memo: eventEl.memo,
                    className: $(eventEl).data('class'),
                };
            }
        });

        var $this = this;

        $this.$calendarObj = new FullCalendar.Calendar($this.$calendar[0], {
            timeZone: 'Local',
            plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid', 'list'],
            slotDuration: '00:15:00', /* If we want to split day time each 15minutes */
            minTime: '07:00:00',
            maxTime: '19:00:00',
            themeSystem: 'bootstrap',
            bootstrapFontAwesome: false,
            buttonText: {
                today: 'Today',
                month: 'Month',
                week: 'Week',
                day: 'Day',
                list: 'List',
                prev: 'Prev',
                next: 'Next'
            },
            defaultView: 'timeGridWeek',
            handleWindowResize: true,
            height: $(window).height() - 200,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
            },
            events: defaultEvents,
            editable: true,
            locale: "ja",
            droppable: true, // this allows things to be dropped onto the calendar !!!
            eventLimit: true, // allow "more" link when too many events
            selectable: true,
            dateClick: function (info) {
                $this.onSelect(info);
                var myModal = new bootstrap.Modal(document.getElementById('event-modal'), {});
                myModal.toggle();
            },
            eventClick: function (info) {
                window.location.href = "schedule?schedule_no=" + info.event.id;
            },
            eventResize: function (info) {
                var data = {
                    schedule_no: info.event.id,
                    allday: info.event.allDay,
                    start_datetime: info.event.start,
                    end_datetime: info.event.end
                }
                $.ajax({
                    url: 'Edit',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(data),
                    success: function (response) {
                        console.log(response)
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            },
            eventDrop: function (info) {
                var data = {
                    schedule_no: info.event.id,
                    allday: info.event.allDay,
                    start_datetime: info.event.start,
                    end_datetime: info.event.end
                }
                $.ajax({
                    url: 'Edit',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(data),
                    success: function (response) {
                        console.log(response)
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            },
        });

        $this.$calendarObj.render();

        $this.$allDay.change(function () {
            // Get the checked state of the checkbox
            var isChecked = $(this).is(':checked');

            if (isChecked) {
                $('#daytime select').prop('disabled', true);
            } else {
                $('#daytime select').prop('disabled', false);
            }
        });

        $this.$btnNewEvent.on('click', function (e) {
            var url = 'Schedule_Create';
            window.location.href = url;
        });

        // save event
        $this.$formEvent.on('submit', function (e) {
            e.preventDefault();
            var form = $this.$formEvent[0];

            // validation
            if (form.checkValidity()) {
                if ($this.$selectedEvent) {
                    var startDatetime = new Date($this.$selectedEvent.start);
                    var endDatetime = new Date($this.$selectedEvent.end ? $this.$selectedEvent.end : $this.$selectedEvent.start);

                    startDatetime.setHours(parseInt($this.$startHour.val()));
                    startDatetime.setMinutes(parseInt($this.$startMinute.val()));

                    endDatetime.setHours(parseInt($this.$endHour.val()));
                    endDatetime.setMinutes(parseInt($this.$endMinute.val()));
                    var data = {
                        schedule_no: $this.$selectedEvent.id,
                        schedule_type: $("#event-category").val(),
                        allday: $("#allDay").prop("checked"),
                        start_datetime: startDatetime,
                        end_datetime: endDatetime,
                        title: $("#event-title").val(),
                        memo: $("#event-memo").val(),
                        creator: 1,
                    }
                    $.ajax({
                        url: 'Edit',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(data),
                        success: function (response) {
                            var className = '';
                            switch (parseInt([$("#event-category").val()])) {
                                case 1:
                                    className = 'bg-success';
                                    break;
                                case 2:
                                    className = 'bg-info';
                                    break;
                                case 3:
                                    className = 'bg-warning';
                                    break;
                                case 4:
                                    className = 'bg-purple';
                                    break;
                                case 5:
                                    className = 'bg-danger';
                                    break;
                                default:
                                    className = '';
                                    break;
                            }
                            var up_data = {
                                id: $this.$selectedEvent.id,
                                classNames: className,
                                allDay: $("#allDay").prop("checked"),
                                start: startDatetime,
                                end: endDatetime,
                                title: $("#event-title").val(),
                                memo: $("#event-memo").val(),
                                creator: 1,
                            }
                            $this.$selectedEvent.remove();
                            $this.$calendarObj.addEvent(up_data);
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                } else {
                    var startDatetime = new Date($this.$newEventData.date);
                    var endDatetime = new Date($this.$newEventData.date);
                    startDatetime.setHours(parseInt($this.$startHour.val()));
                    startDatetime.setMinutes(parseInt($this.$startMinute.val()));
                    var start = new Date(startDatetime.getTime() - 8000 * 3600).toISOString();
                    var end = new Date(endDatetime.getTime() - 8000 * 3600).toISOString();
                    var data = {
                        schedule_type: parseInt($("#event-category").val()),
                        allday: $("#allDay").prop("checked"),
                        start_datetime: start,
                        end_datetime: end,
                        title: $("#event-title").val(),
                        memo: $("#event-memo").val(),
                    };
                    $.ajax({
                        url: 'Create',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(data),
                        success: function (response) {
                            const { schedule_no, schedule_type, allday, start_datetime, end_datetime, title, memo } = response;
                            const start = new Date(start_datetime);
                            const end = new Date(end_datetime);
                            var className = '';
                            switch (parseInt(schedule_type)) {
                                case -1:
                                    className = 'bg-info';
                                    break;
                                case 2:
                                    className = 'bg-success';
                                    break;
                                case 3:
                                    className = 'bg-warning';
                                    break;
                                case 4:
                                    className = 'bg-purple';
                                    break;
                                case 5:
                                    className = 'bg-danger';
                                    break;
                                default:
                                    className = '';
                                    break;
                            }
                            var data = {
                                id: schedule_no,
                                title,
                                memo,
                                allDay: allday,
                                start,
                                end,
                                className,
                            };
                            $this.$calendarObj.addEvent(data);
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }
                $this.$modal.modal('hide');
            } else {
                e.stopPropagation();
                form.classList.add('was-validated');
            }
        });
        // delete event
        $this.$btnDeleteEvent.on('click', function (e) {
            if ($this.$selectedEvent) {
                $.ajax({
                    url: 'Delete/' + $this.$selectedEvent.id,
                    type: 'POST',
                    contentType: 'application/json',
                    success: function (response) {
                        $this.$selectedEvent.remove();
                        $this.$selectedEvent = null;
                        $this.$modal.modal('hide');
                    },
                    error: (err) => {
                        console.log(err)
                    }

                });
            }
        });
    },

    //init CalendarApp
    $.CalendarApp = new CalendarApp, $.CalendarApp.Constructor = CalendarApp

    //$.CalendarApp.init()

}(window.jQuery),
(async function ($) {
    "use strict";
    $.CalendarApp.init(await get_results("G-0"));
    
})(window.jQuery);

$("#filterSelect").on('change', async (e) => {
    var filter = e.target.value != null ? e.target.value : "G-0";
    $("#calendar").empty();
    $.CalendarApp.init(await get_results(filter));
});

function get_results(filter) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: 'getGroup?cond=' + filter,
            type: 'POST',
            contentType: 'application/json',
            success: function (response) {
                const modifiedData = response.map((item) => {
                    const { schedule_no, schedule_type, allday, start_datetime, end_datetime, title, memo } = item;
                    const sss = new Date(start_datetime);
                    const start = new Date(sss.getTime() - 3600 * 8000);
                    const eee = new Date(end_datetime);
                    const end = new Date(eee.getTime()-3600*8000);
                    var className = '';

                    switch (parseInt(schedule_type)) {
                        case 1:
                            className = 'bg-info';
                            break;
                        case 2:
                            className = 'bg-success';
                            break;
                        case 3:
                            className = 'bg-warning';
                            break;
                        case 4:
                            className = 'bg-purple';
                            break;
                        case 5:
                            className = 'bg-danger';
                            break;
                        default:
                            className = '';
                            break;
                    }
                    return {
                        id: schedule_no,
                        title,
                        memo,
                        allDay: allday,
                        start,
                        end,
                        className,
                    };
                });
                resolve(modifiedData);
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}

!function ($) {
    const startHourSelect = document.getElementById('startHour');
    const startMinuteSelect = document.getElementById('startMinute');
    const endHourSelect = document.getElementById('endHour');
    const endMinuteSelect = document.getElementById('endMinute');

    startHourSelect.addEventListener('change', updateEndTime);
    startMinuteSelect.addEventListener('change', updateEndTime);
    endHourSelect.addEventListener('change', updateEndMinute);

    function updateEndTime() {
        
        const startHour = parseInt(startHourSelect.value);
        const startMinute = parseInt(startMinuteSelect.value);

        let endHour = parseInt(endHourSelect.value);
        let endMinute = parseInt(endMinuteSelect.value);

        for (let i = 0; i < endHourSelect.options.length; i++) {
            const option = endHourSelect.options[i];
            option.disabled = (startHour > endHourSelect.options[i].value);
        }

        for (let i = 0; i < endMinuteSelect.options.length; i++) {
            const option = endMinuteSelect.options[i];
            const optionValue = parseInt(option.value);
            option.disabled = startHour === endHour && optionValue < startMinute;
        }

        const minEndHour = startHour + Math.ceil((startMinute + 1) / 60);
        const minEndMinute = (Math.ceil((startMinute + 5) / 5) % 12) * 5;

        if (endHour < minEndHour || (endHour === minEndHour && endMinute < minEndMinute)) {
            endHour = minEndHour;
            endMinute = startMinute;

            endHourSelect.value = endHour.toString();
            for (let i = 0; i < minEndHour; i++) {
                endHourSelect.op
            }
            endMinuteSelect.value = endMinute.toString();
        }
    }

    function updateEndMinute() {
        const startHour = parseInt(startHourSelect.value);
        const startMinute = parseInt(startMinuteSelect.value);
        let endHour = parseInt(endHourSelect.value);

        for (let i = 0; i < endMinuteSelect.options.length; i++) {
            const option = endMinuteSelect.options[i];
            const optionValue = parseInt(option.value);
            option.disabled = startHour === endHour && optionValue < startMinute;
        }
    }

}(window.jQuery)


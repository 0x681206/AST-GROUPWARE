/*!
FullCalendar Time Grid Plugin v4.4.2
Docs & License: https://fullcalendar.io/
(c) 2019 Adam Shaw
*/
!(function (e, t) {
    "object" == typeof exports && "undefined" != typeof module
        ? t(exports, require("@fullcalendar/core"), require("@fullcalendar/daygrid"))
        : "function" == typeof define && define.amd
        ? define(["exports", "@fullcalendar/core", "@fullcalendar/daygrid"], t)
        : t(((e = e || self).FullCalendarTimeGrid = {}), e.FullCalendar, e.FullCalendarDayGrid);
})(this, function (e, t, r) {
    "use strict";
    var i = function (e, t) {
        return (i =
            Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array &&
                function (e, t) {
                    e.__proto__ = t;
                }) ||
            function (e, t) {
                for (var r in t) t.hasOwnProperty(r) && (e[r] = t[r]);
            })(e, t);
    };
    function n(e, t) {
        function r() {
            this.constructor = e;
        }
        i(e, t), (e.prototype = null === t ? Object.create(t) : ((r.prototype = t.prototype), new r()));
    }
    var o = function () {
            return (o =
                Object.assign ||
                function (e) {
                    for (var t, r = 1, i = arguments.length; r < i; r++) for (var n in (t = arguments[r])) Object.prototype.hasOwnProperty.call(t, n) && (e[n] = t[n]);
                    return e;
                }).apply(this, arguments);
        },
        s = (function (e) {
            function r(t) {
                var r = e.call(this) || this;
                return (r.timeGrid = t), r;
            }
            return (
                n(r, e),
                (r.prototype.renderSegs = function (r, i, n) {
                    e.prototype.renderSegs.call(this, r, i, n), (this.fullTimeFormat = t.createFormatter({ hour: "numeric", minute: "2-digit", separator: this.context.options.defaultRangeSeparator }));
                }),
                (r.prototype.attachSegs = function (e, t) {
                    for (var r = this.timeGrid.groupSegsByCol(e), i = 0; i < r.length; i++) r[i] = this.sortEventSegs(r[i]);
                    (this.segsByCol = r), this.timeGrid.attachSegsByCol(r, this.timeGrid.fgContainerEls);
                }),
                (r.prototype.detachSegs = function (e) {
                    e.forEach(function (e) {
                        t.removeElement(e.el);
                    }),
                        (this.segsByCol = null);
                }),
                (r.prototype.computeSegSizes = function (e) {
                    var t = this.timeGrid,
                        r = this.segsByCol,
                        i = t.colCnt;
                    if ((t.computeSegVerticals(e), r)) for (var n = 0; n < i; n++) this.computeSegHorizontals(r[n]);
                }),
                (r.prototype.assignSegSizes = function (e) {
                    var t = this.timeGrid,
                        r = this.segsByCol,
                        i = t.colCnt;
                    if ((t.assignSegVerticals(e), r)) for (var n = 0; n < i; n++) this.assignSegCss(r[n]);
                }),
                (r.prototype.computeEventTimeFormat = function () {
                    return { hour: "numeric", minute: "2-digit", meridiem: !1 };
                }),
                (r.prototype.computeDisplayEventEnd = function () {
                    return !0;
                }),
                (r.prototype.renderSegHtml = function (e, r) {
                    var i,
                        n,
                        o,
                        s = e.eventRange,
                        a = s.def,
                        l = s.ui,
                        d = a.allDay,
                        c = t.computeEventDraggable(this.context, a, l),
                        h = e.isStart && t.computeEventStartResizable(this.context, a, l),
                        u = e.isEnd && t.computeEventEndResizable(this.context, a, l),
                        p = this.getSegClasses(e, c, h || u, r),
                        f = t.cssToStr(this.getSkinCss(l));
                    if ((p.unshift("fc-time-grid-event"), t.isMultiDayRange(s.range))) {
                        if (e.isStart || e.isEnd) {
                            var g = e.start,
                                m = e.end;
                            (i = this._getTimeText(g, m, d)), (n = this._getTimeText(g, m, d, this.fullTimeFormat)), (o = this._getTimeText(g, m, d, null, !1));
                        }
                    } else (i = this.getTimeText(s)), (n = this.getTimeText(s, this.fullTimeFormat)), (o = this.getTimeText(s, null, !1));
                    //console.log(a)
                    return (
                        '<a class="' +
                        p.join(" ") +
                        '"' +
                        (a.url ? ' href="' + t.htmlEscape(a.url) + '"' : "") +
                        (f ? ' style="' + f + '"' : "") +
                        '><div class="fc-content">' +
                        (i ? '<div class="fc-time" data-start="' + t.htmlEscape(o) + '" data-full="' + t.htmlEscape(n) + '"><span>' + t.htmlEscape(i) + "</span></div>" : "") +
                        (a.title ? '<div class="fc-title">' + t.htmlEscape(a.title) + "</div>" : "") + "<hr>" +
                        (a.memo ? '<div class="fc-memo">' + t.htmlEscape(a.memo) + "</div>" : "") +
                        "</div>" +
                        (u ? '<div class="fc-resizer fc-end-resizer"></div>' : "") +
                        "</a>"
                    );
                }),
                (r.prototype.computeSegHorizontals = function (e) {
                    var t, r, i;
                    if (
                        ((function (e) {
                            var t, r, i, n, o;
                            for (t = 0; t < e.length; t++) for (r = e[t], i = 0; i < r.length; i++) for ((n = r[i]).forwardSegs = [], o = t + 1; o < e.length; o++) l(n, e[o], n.forwardSegs);
                        })(
                            (t = (function (e) {
                                var t,
                                    r,
                                    i,
                                    n = [];
                                for (t = 0; t < e.length; t++) {
                                    for (r = e[t], i = 0; i < n.length && l(r, n[i]).length; i++);
                                    (r.level = i), (n[i] || (n[i] = [])).push(r);
                                }
                                return n;
                            })(e))
                        ),
                        (r = t[0]))
                    ) {
                        for (i = 0; i < r.length; i++) a(r[i]);
                        for (i = 0; i < r.length; i++) this.computeSegForwardBack(r[i], 0, 0);
                    }
                }),
                (r.prototype.computeSegForwardBack = function (e, t, r) {
                    var i,
                        n = e.forwardSegs;
                    if (void 0 === e.forwardCoord)
                        for (
                            n.length ? (this.sortForwardSegs(n), this.computeSegForwardBack(n[0], t + 1, r), (e.forwardCoord = n[0].backwardCoord)) : (e.forwardCoord = 1),
                                e.backwardCoord = e.forwardCoord - (e.forwardCoord - r) / (t + 1),
                                i = 0;
                            i < n.length;
                            i++
                        )
                            this.computeSegForwardBack(n[i], 0, e.forwardCoord);
                }),
                (r.prototype.sortForwardSegs = function (e) {
                    var r = e.map(d),
                        i = [
                            { field: "forwardPressure", order: -1 },
                            { field: "backwardCoord", order: 1 },
                        ].concat(this.context.eventOrderSpecs);
                    return (
                        r.sort(function (e, r) {
                            return t.compareByFieldSpecs(e, r, i);
                        }),
                        r.map(function (e) {
                            return e._seg;
                        })
                    );
                }),
                (r.prototype.assignSegCss = function (e) {
                    for (var r = 0, i = e; r < i.length; r++) {
                        var n = i[r];
                        t.applyStyle(n.el, this.generateSegCss(n)), n.level > 0 && n.el.classList.add("fc-time-grid-event-inset"), n.eventRange.def.title && n.bottom - n.top < 30 && n.el.classList.add("fc-short");
                    }
                }),
                (r.prototype.generateSegCss = function (e) {
                    var t,
                        r,
                        i = this.context.options.slotEventOverlap,
                        n = e.backwardCoord,
                        o = e.forwardCoord,
                        s = this.timeGrid.generateSegVerticalCss(e),
                        a = this.context.isRtl;
                    return (
                        i && (o = Math.min(1, n + 2 * (o - n))),
                        a ? ((t = 1 - o), (r = n)) : ((t = n), (r = 1 - o)),
                        (s.zIndex = e.level + 1),
                        (s.left = 100 * t + "%"),
                        (s.right = 100 * r + "%"),
                        i && e.forwardPressure && (s[a ? "marginLeft" : "marginRight"] = 20),
                        s
                    );
                }),
                r
            );
        })(t.FgEventRenderer);
    function a(e) {
        var t,
            r,
            i = e.forwardSegs,
            n = 0;
        if (void 0 === e.forwardPressure) {
            for (t = 0; t < i.length; t++) a((r = i[t])), (n = Math.max(n, 1 + r.forwardPressure));
            e.forwardPressure = n;
        }
    }
    function l(e, t, r) {
        void 0 === r && (r = []);
        for (var i = 0; i < t.length; i++) (n = e), (o = t[i]), n.bottom > o.top && n.top < o.bottom && r.push(t[i]);
        var n, o;
        return r;
    }
    function d(e) {
        var r = t.buildSegCompareObj(e);
        return (r.forwardPressure = e.forwardPressure), (r.backwardCoord = e.backwardCoord), r;
    }
    var c = (function (e) {
            function t() {
                return (null !== e && e.apply(this, arguments)) || this;
            }
            return (
                n(t, e),
                (t.prototype.attachSegs = function (e, t) {
                    (this.segsByCol = this.timeGrid.groupSegsByCol(e)), this.timeGrid.attachSegsByCol(this.segsByCol, this.timeGrid.mirrorContainerEls), (this.sourceSeg = t.sourceSeg);
                }),
                (t.prototype.generateSegCss = function (t) {
                    var r = e.prototype.generateSegCss.call(this, t),
                        i = this.sourceSeg;
                    if (i && i.col === t.col) {
                        var n = e.prototype.generateSegCss.call(this, i);
                        (r.left = n.left), (r.right = n.right), (r.marginLeft = n.marginLeft), (r.marginRight = n.marginRight);
                    }
                    return r;
                }),
                t
            );
        })(s),
        h = (function (e) {
            function t(t) {
                var r = e.call(this) || this;
                return (r.timeGrid = t), r;
            }
            return (
                n(t, e),
                (t.prototype.attachSegs = function (e, t) {
                    var r,
                        i = this.timeGrid;
                    return (
                        "bgEvent" === e ? (r = i.bgContainerEls) : "businessHours" === e ? (r = i.businessContainerEls) : "highlight" === e && (r = i.highlightContainerEls),
                        i.attachSegsByCol(i.groupSegsByCol(t), r),
                        t.map(function (e) {
                            return e.el;
                        })
                    );
                }),
                (t.prototype.computeSegSizes = function (e) {
                    this.timeGrid.computeSegVerticals(e);
                }),
                (t.prototype.assignSegSizes = function (e) {
                    this.timeGrid.assignSegVerticals(e);
                }),
                t
            );
        })(t.FillRenderer),
        u = [{ hours: 1 }, { minutes: 30 }, { minutes: 15 }, { seconds: 30 }, { seconds: 15 }],
        p = (function (e) {
            function i(r, i) {
                var n = e.call(this, r) || this;
                (n.isSlatSizesDirty = !1),
                    (n.isColSizesDirty = !1),
                    (n.processOptions = t.memoize(n._processOptions)),
                    (n.renderSkeleton = t.memoizeRendering(n._renderSkeleton)),
                    (n.renderSlats = t.memoizeRendering(n._renderSlats, null, [n.renderSkeleton])),
                    (n.renderColumns = t.memoizeRendering(n._renderColumns, n._unrenderColumns, [n.renderSkeleton])),
                    (n.renderProps = i);
                var o = n.renderColumns,
                    a = (n.eventRenderer = new s(n)),
                    l = (n.fillRenderer = new h(n));
                return (
                    (n.mirrorRenderer = new c(n)),
                    (n.renderBusinessHours = t.memoizeRendering(l.renderSegs.bind(l, "businessHours"), l.unrender.bind(l, "businessHours"), [o])),
                    (n.renderDateSelection = t.memoizeRendering(n._renderDateSelection, n._unrenderDateSelection, [o])),
                    (n.renderFgEvents = t.memoizeRendering(a.renderSegs.bind(a), a.unrender.bind(a), [o])),
                    (n.renderBgEvents = t.memoizeRendering(l.renderSegs.bind(l, "bgEvent"), l.unrender.bind(l, "bgEvent"), [o])),
                    (n.renderEventSelection = t.memoizeRendering(a.selectByInstanceId.bind(a), a.unselectByInstanceId.bind(a), [n.renderFgEvents])),
                    (n.renderEventDrag = t.memoizeRendering(n._renderEventDrag, n._unrenderEventDrag, [o])),
                    (n.renderEventResize = t.memoizeRendering(n._renderEventResize, n._unrenderEventResize, [o])),
                    n
                );
            }
            return (
                n(i, e),
                (i.prototype._processOptions = function (e) {
                    var r,
                        i,
                        n = e.slotDuration,
                        o = e.snapDuration;
                    (n = t.createDuration(n)),
                        (o = o ? t.createDuration(o) : n),
                        null === (r = t.wholeDivideDurations(n, o)) && ((o = n), (r = 1)),
                        (this.slotDuration = n),
                        (this.snapDuration = o),
                        (this.snapsPerSlot = r),
                        (i = e.slotLabelFormat),
                        Array.isArray(i) && (i = i[i.length - 1]),
                        (this.labelFormat = t.createFormatter(i || { hour: "numeric", minute: "2-digit", omitZeroMinute: !0, meridiem: "short" })),
                        (i = e.slotLabelInterval),
                        (this.labelInterval = i ? t.createDuration(i) : this.computeLabelInterval(n));
                }),
                (i.prototype.computeLabelInterval = function (e) {
                    var r, i, n;
                    for (r = u.length - 1; r >= 0; r--) if (((i = t.createDuration(u[r])), null !== (n = t.wholeDivideDurations(i, e)) && n > 1)) return i;
                    return e;
                }),
                (i.prototype.render = function (e, t) {
                    this.processOptions(t.options);
                    var r = e.cells;
                    (this.colCnt = r.length),
                        this.renderSkeleton(t.theme),
                        this.renderSlats(e.dateProfile),
                        this.renderColumns(e.cells, e.dateProfile),
                        this.renderBusinessHours(t, e.businessHourSegs),
                        this.renderDateSelection(e.dateSelectionSegs),
                        this.renderFgEvents(t, e.fgEventSegs),
                        this.renderBgEvents(t, e.bgEventSegs),
                        this.renderEventSelection(e.eventSelection),
                        this.renderEventDrag(e.eventDrag),
                        this.renderEventResize(e.eventResize);
                }),
                (i.prototype.destroy = function () {
                    e.prototype.destroy.call(this), this.renderSlats.unrender(), this.renderColumns.unrender(), this.renderSkeleton.unrender();
                }),
                (i.prototype.updateSize = function (e) {
                    var t = this.fillRenderer,
                        r = this.eventRenderer,
                        i = this.mirrorRenderer;
                    (e || this.isSlatSizesDirty) && (this.buildSlatPositions(), (this.isSlatSizesDirty = !1)),
                        (e || this.isColSizesDirty) && (this.buildColPositions(), (this.isColSizesDirty = !1)),
                        t.computeSizes(e),
                        r.computeSizes(e),
                        i.computeSizes(e),
                        t.assignSizes(e),
                        r.assignSizes(e),
                        i.assignSizes(e);
                }),
                (i.prototype._renderSkeleton = function (e) {
                    var t = this.el;
                    (t.innerHTML = '<div class="fc-bg"></div><div class="fc-slats"></div><hr class="fc-divider ' + e.getClass("widgetHeader") + '" style="display:none" />'),
                        (this.rootBgContainerEl = t.querySelector(".fc-bg")),
                        (this.slatContainerEl = t.querySelector(".fc-slats")),
                        (this.bottomRuleEl = t.querySelector(".fc-divider"));
                }),
                (i.prototype._renderSlats = function (e) {
                    var r = this.context.theme;
                    (this.slatContainerEl.innerHTML = '<table class="' + r.getClass("tableGrid") + '">' + this.renderSlatRowHtml(e) + "</table>"),
                        (this.slatEls = t.findElements(this.slatContainerEl, "tr")),
                        (this.slatPositions = new t.PositionCache(this.el, this.slatEls, !1, !0)),
                        (this.isSlatSizesDirty = !0);
                }),
                (i.prototype.renderSlatRowHtml = function (e) {
                    for (var r, i, n, o = this.context, s = o.dateEnv, a = o.theme, l = o.isRtl, d = "", c = t.startOfDay(e.renderRange.start), h = e.minTime, u = t.createDuration(0); t.asRoughMs(h) < t.asRoughMs(e.maxTime); )
                        (r = s.add(c, h)),
                            (i = null !== t.wholeDivideDurations(u, this.labelInterval)),
                            (n = '<td class="fc-axis fc-time ' + a.getClass("widgetContent") + '">' + (i ? "<span>" + t.htmlEscape(s.format(r, this.labelFormat)) + "</span>" : "") + "</td>"),
                            (d += '<tr data-time="' + t.formatIsoTimeString(r) + '"' + (i ? "" : ' class="fc-minor"') + ">" + (l ? "" : n) + '<td class="' + a.getClass("widgetContent") + '"></td>' + (l ? n : "") + "</tr>"),
                            (h = t.addDurations(h, this.slotDuration)),
                            (u = t.addDurations(u, this.slotDuration));
                    return d;
                }),
                (i.prototype._renderColumns = function (e, i) {
                    var n = this.context,
                        o = n.calendar,
                        s = n.view,
                        a = n.isRtl,
                        l = n.theme,
                        d = n.dateEnv,
                        c = new r.DayBgRow(this.context);
                    (this.rootBgContainerEl.innerHTML = '<table class="' + l.getClass("tableGrid") + '">' + c.renderHtml({ cells: e, dateProfile: i, renderIntroHtml: this.renderProps.renderBgIntroHtml }) + "</table>"),
                        (this.colEls = t.findElements(this.el, ".fc-day, .fc-disabled-day"));
                    for (var h = 0; h < this.colCnt; h++) o.publiclyTrigger("dayRender", [{ date: d.toDate(e[h].date), el: this.colEls[h], view: s }]);
                    a && this.colEls.reverse(), (this.colPositions = new t.PositionCache(this.el, this.colEls, !0, !1)), this.renderContentSkeleton(), (this.isColSizesDirty = !0);
                }),
                (i.prototype._unrenderColumns = function () {
                    this.unrenderContentSkeleton();
                }),
                (i.prototype.renderContentSkeleton = function () {
                    var e,
                        r = this.context.isRtl,
                        i = [];
                    i.push(this.renderProps.renderIntroHtml());
                    for (var n = 0; n < this.colCnt; n++)
                        i.push(
                            '<td><div class="fc-content-col"><div class="fc-event-container fc-mirror-container"></div><div class="fc-event-container"></div><div class="fc-highlight-container"></div><div class="fc-bgevent-container"></div><div class="fc-business-container"></div></div></td>'
                        );
                    r && i.reverse(),
                        (e = this.contentSkeletonEl = t.htmlToElement('<div class="fc-content-skeleton"><table><tr>' + i.join("") + "</tr></table></div>")),
                        (this.colContainerEls = t.findElements(e, ".fc-content-col")),
                        (this.mirrorContainerEls = t.findElements(e, ".fc-mirror-container")),
                        (this.fgContainerEls = t.findElements(e, ".fc-event-container:not(.fc-mirror-container)")),
                        (this.bgContainerEls = t.findElements(e, ".fc-bgevent-container")),
                        (this.highlightContainerEls = t.findElements(e, ".fc-highlight-container")),
                        (this.businessContainerEls = t.findElements(e, ".fc-business-container")),
                        r && (this.colContainerEls.reverse(), this.mirrorContainerEls.reverse(), this.fgContainerEls.reverse(), this.bgContainerEls.reverse(), this.highlightContainerEls.reverse(), this.businessContainerEls.reverse()),
                        this.el.appendChild(e);
                }),
                (i.prototype.unrenderContentSkeleton = function () {
                    t.removeElement(this.contentSkeletonEl);
                }),
                (i.prototype.groupSegsByCol = function (e) {
                    var t,
                        r = [];
                    for (t = 0; t < this.colCnt; t++) r.push([]);
                    for (t = 0; t < e.length; t++) r[e[t].col].push(e[t]);
                    return r;
                }),
                (i.prototype.attachSegsByCol = function (e, t) {
                    var r, i, n;
                    for (r = 0; r < this.colCnt; r++) for (i = e[r], n = 0; n < i.length; n++) t[r].appendChild(i[n].el);
                }),
                (i.prototype.getNowIndicatorUnit = function () {
                    return "minute";
                }),
                (i.prototype.renderNowIndicator = function (e, r) {
                    if (this.colContainerEls) {
                        var i,
                            n = this.computeDateTop(r),
                            o = [];
                        for (i = 0; i < e.length; i++) {
                            var s = t.createElement("div", { className: "fc-now-indicator fc-now-indicator-line" });
                            (s.style.top = n + "px"), this.colContainerEls[e[i].col].appendChild(s), o.push(s);
                        }
                        if (e.length > 0) {
                            var a = t.createElement("div", { className: "fc-now-indicator fc-now-indicator-arrow" });
                            (a.style.top = n + "px"), this.contentSkeletonEl.appendChild(a), o.push(a);
                        }
                        this.nowIndicatorEls = o;
                    }
                }),
                (i.prototype.unrenderNowIndicator = function () {
                    this.nowIndicatorEls && (this.nowIndicatorEls.forEach(t.removeElement), (this.nowIndicatorEls = null));
                }),
                (i.prototype.getTotalSlatHeight = function () {
                    return this.slatContainerEl.getBoundingClientRect().height;
                }),
                (i.prototype.computeDateTop = function (e, r) {
                    return r || (r = t.startOfDay(e)), this.computeTimeTop(t.createDuration(e.valueOf() - r.valueOf()));
                }),
                (i.prototype.computeTimeTop = function (e) {
                    var r,
                        i,
                        n = this.slatEls.length,
                        o = this.props.dateProfile,
                        s = (e.milliseconds - t.asRoughMs(o.minTime)) / t.asRoughMs(this.slotDuration);
                    return (s = Math.max(0, s)), (s = Math.min(n, s)), (r = Math.floor(s)), (i = s - (r = Math.min(r, n - 1))), this.slatPositions.tops[r] + this.slatPositions.getHeight(r) * i;
                }),
                (i.prototype.computeSegVerticals = function (e) {
                    var t,
                        r,
                        i,
                        n = this.context.options.timeGridEventMinHeight;
                    for (t = 0; t < e.length; t++) (r = e[t]), (i = this.props.cells[r.col].date), (r.top = this.computeDateTop(r.start, i)), (r.bottom = Math.max(r.top + n, this.computeDateTop(r.end, i)));
                }),
                (i.prototype.assignSegVerticals = function (e) {
                    var r, i;
                    for (r = 0; r < e.length; r++) (i = e[r]), t.applyStyle(i.el, this.generateSegVerticalCss(i));
                }),
                (i.prototype.generateSegVerticalCss = function (e) {
                    return { top: e.top, bottom: -e.bottom };
                }),
                (i.prototype.buildPositionCaches = function () {
                    this.buildColPositions(), this.buildSlatPositions();
                }),
                (i.prototype.buildColPositions = function () {
                    this.colPositions.build();
                }),
                (i.prototype.buildSlatPositions = function () {
                    this.slatPositions.build();
                }),
                (i.prototype.positionToHit = function (e, r) {
                    var i = this.context.dateEnv,
                        n = this.snapsPerSlot,
                        o = this.slatPositions,
                        s = this.colPositions,
                        a = s.leftToIndex(e),
                        l = o.topToIndex(r);
                    if (null != a && null != l) {
                        var d = o.tops[l],
                            c = o.getHeight(l),
                            h = (r - d) / c,
                            u = l * n + Math.floor(h * n),
                            p = this.props.cells[a].date,
                            f = t.addDurations(this.props.dateProfile.minTime, t.multiplyDuration(this.snapDuration, u)),
                            g = i.add(p, f);
                        return { col: a, dateSpan: { range: { start: g, end: i.add(g, this.snapDuration) }, allDay: !1 }, dayEl: this.colEls[a], relativeRect: { left: s.lefts[a], right: s.rights[a], top: d, bottom: d + c } };
                    }
                }),
                (i.prototype._renderEventDrag = function (e) {
                    e &&
                        (this.eventRenderer.hideByHash(e.affectedInstances),
                        e.isEvent ? this.mirrorRenderer.renderSegs(this.context, e.segs, { isDragging: !0, sourceSeg: e.sourceSeg }) : this.fillRenderer.renderSegs("highlight", this.context, e.segs));
                }),
                (i.prototype._unrenderEventDrag = function (e) {
                    e &&
                        (this.eventRenderer.showByHash(e.affectedInstances),
                        e.isEvent ? this.mirrorRenderer.unrender(this.context, e.segs, { isDragging: !0, sourceSeg: e.sourceSeg }) : this.fillRenderer.unrender("highlight", this.context));
                }),
                (i.prototype._renderEventResize = function (e) {
                    e && (this.eventRenderer.hideByHash(e.affectedInstances), this.mirrorRenderer.renderSegs(this.context, e.segs, { isResizing: !0, sourceSeg: e.sourceSeg }));
                }),
                (i.prototype._unrenderEventResize = function (e) {
                    e && (this.eventRenderer.showByHash(e.affectedInstances), this.mirrorRenderer.unrender(this.context, e.segs, { isResizing: !0, sourceSeg: e.sourceSeg }));
                }),
                (i.prototype._renderDateSelection = function (e) {
                    e && (this.context.options.selectMirror ? this.mirrorRenderer.renderSegs(this.context, e, { isSelecting: !0 }) : this.fillRenderer.renderSegs("highlight", this.context, e));
                }),
                (i.prototype._unrenderDateSelection = function (e) {
                    e && (this.context.options.selectMirror ? this.mirrorRenderer.unrender(this.context, e, { isSelecting: !0 }) : this.fillRenderer.unrender("highlight", this.context));
                }),
                i
            );
        })(t.DateComponent),
        f = (function (e) {
            function r() {
                return (null !== e && e.apply(this, arguments)) || this;
            }
            return (
                n(r, e),
                (r.prototype.getKeyInfo = function () {
                    return { allDay: {}, timed: {} };
                }),
                (r.prototype.getKeysForDateSpan = function (e) {
                    return e.allDay ? ["allDay"] : ["timed"];
                }),
                (r.prototype.getKeysForEventDef = function (e) {
                    return e.allDay ? (t.hasBgRendering(e) ? ["timed", "allDay"] : ["allDay"]) : ["timed"];
                }),
                r
            );
        })(t.Splitter),
        g = t.createFormatter({ week: "short" }),
        m = (function (e) {
            function i() {
                var r = (null !== e && e.apply(this, arguments)) || this;
                return (
                    (r.splitter = new f()),
                    (r.renderSkeleton = t.memoizeRendering(r._renderSkeleton, r._unrenderSkeleton)),
                    (r.renderHeadIntroHtml = function () {
                        var e,
                            i = r.context,
                            n = i.theme,
                            o = i.dateEnv,
                            s = i.options,
                            a = r.props.dateProfile.renderRange,
                            l = t.diffDays(a.start, a.end);
                        return s.weekNumbers
                            ? ((e = o.format(a.start, g)),
                              '<th class="fc-axis fc-week-number ' + n.getClass("widgetHeader") + '" ' + r.axisStyleAttr() + ">" + t.buildGotoAnchorHtml(s, o, { date: a.start, type: "week", forceOff: l > 1 }, t.htmlEscape(e)) + "</th>")
                            : '<th class="fc-axis ' + n.getClass("widgetHeader") + '" ' + r.axisStyleAttr() + "></th>";
                    }),
                    (r.renderTimeGridBgIntroHtml = function () {
                        return '<td class="fc-axis ' + r.context.theme.getClass("widgetContent") + '" ' + r.axisStyleAttr() + "></td>";
                    }),
                    (r.renderTimeGridIntroHtml = function () {
                        return '<td class="fc-axis" ' + r.axisStyleAttr() + "></td>";
                    }),
                    (r.renderDayGridBgIntroHtml = function () {
                        var e = r.context,
                            i = e.theme,
                            n = e.options;
                        return '<td class="fc-axis ' + i.getClass("widgetContent") + '" ' + r.axisStyleAttr() + "><span>" + t.getAllDayHtml(n) + "</span></td>";
                    }),
                    (r.renderDayGridIntroHtml = function () {
                        return '<td class="fc-axis" ' + r.axisStyleAttr() + "></td>";
                    }),
                    r
                );
            }
            return (
                n(i, e),
                (i.prototype.render = function (t, r) {
                    e.prototype.render.call(this, t, r), this.renderSkeleton(r);
                }),
                (i.prototype.destroy = function () {
                    e.prototype.destroy.call(this), this.renderSkeleton.unrender();
                }),
                (i.prototype._renderSkeleton = function (e) {
                    this.el.classList.add("fc-timeGrid-view"), (this.el.innerHTML = this.renderSkeletonHtml()), (this.scroller = new t.ScrollComponent("hidden", "auto"));
                    var i = this.scroller.el;
                    this.el.querySelector(".fc-body > tr > td").appendChild(i), i.classList.add("fc-time-grid-container");
                    var n = t.createElement("div", { className: "fc-time-grid" });
                    if ((i.appendChild(n), (this.timeGrid = new p(n, { renderBgIntroHtml: this.renderTimeGridBgIntroHtml, renderIntroHtml: this.renderTimeGridIntroHtml })), e.options.allDaySlot)) {
                        this.dayGrid = new r.DayGrid(this.el.querySelector(".fc-day-grid"), {
                            renderNumberIntroHtml: this.renderDayGridIntroHtml,
                            renderBgIntroHtml: this.renderDayGridBgIntroHtml,
                            renderIntroHtml: this.renderDayGridIntroHtml,
                            colWeekNumbersVisible: !1,
                            cellWeekNumbersVisible: !1,
                        });
                        var o = this.el.querySelector(".fc-divider");
                        this.dayGrid.bottomCoordPadding = o.getBoundingClientRect().height;
                    }
                }),
                (i.prototype._unrenderSkeleton = function () {
                    this.el.classList.remove("fc-timeGrid-view"), this.timeGrid.destroy(), this.dayGrid && this.dayGrid.destroy(), this.scroller.destroy();
                }),
                (i.prototype.renderSkeletonHtml = function () {
                    var e = this.context,
                        t = e.theme,
                        r = e.options;
                    return (
                        '<table class="' +
                        t.getClass("tableGrid") +
                        '">' +
                        (r.columnHeader ? '<thead class="fc-head"><tr><td class="fc-head-container ' + t.getClass("widgetHeader") + '">&nbsp;</td></tr></thead>' : "") +
                        '<tbody class="fc-body"><tr><td class="' +
                        t.getClass("widgetContent") +
                        '">' +
                        (r.allDaySlot ? '<div class="fc-day-grid"></div><hr class="fc-divider ' + t.getClass("widgetHeader") + '" />' : "") +
                        "</td></tr></tbody></table>"
                    );
                }),
                (i.prototype.getNowIndicatorUnit = function () {
                    return this.timeGrid.getNowIndicatorUnit();
                }),
                (i.prototype.unrenderNowIndicator = function () {
                    this.timeGrid.unrenderNowIndicator();
                }),
                (i.prototype.updateSize = function (t, r, i) {
                    e.prototype.updateSize.call(this, t, r, i), this.timeGrid.updateSize(t), this.dayGrid && this.dayGrid.updateSize(t);
                }),
                (i.prototype.updateBaseSize = function (e, r, i) {
                    var n,
                        o,
                        s,
                        a = this;
                    if (((this.axisWidth = t.matchCellWidths(t.findElements(this.el, ".fc-axis"))), this.timeGrid.colEls)) {
                        var l = t.findElements(this.el, ".fc-row").filter(function (e) {
                            return !a.scroller.el.contains(e);
                        });
                        (this.timeGrid.bottomRuleEl.style.display = "none"),
                            this.scroller.clear(),
                            l.forEach(t.uncompensateScroll),
                            this.dayGrid && (this.dayGrid.removeSegPopover(), (n = this.context.options.eventLimit) && "number" != typeof n && (n = 5), n && this.dayGrid.limitRows(n)),
                            i ||
                                ((o = this.computeScrollerHeight(r)),
                                this.scroller.setHeight(o),
                                ((s = this.scroller.getScrollbarWidths()).left || s.right) &&
                                    (l.forEach(function (e) {
                                        t.compensateScroll(e, s);
                                    }),
                                    (o = this.computeScrollerHeight(r)),
                                    this.scroller.setHeight(o)),
                                this.scroller.lockOverflow(s),
                                this.timeGrid.getTotalSlatHeight() < o && (this.timeGrid.bottomRuleEl.style.display = ""));
                    } else i || ((o = this.computeScrollerHeight(r)), this.scroller.setHeight(o));
                }),
                (i.prototype.computeScrollerHeight = function (e) {
                    return e - t.subtractInnerElHeight(this.el, this.scroller.el);
                }),
                (i.prototype.computeDateScroll = function (e) {
                    var t = this.timeGrid.computeTimeTop(e);
                    return (t = Math.ceil(t)) && t++, { top: t };
                }),
                (i.prototype.queryDateScroll = function () {
                    return { top: this.scroller.getScrollTop() };
                }),
                (i.prototype.applyDateScroll = function (e) {
                    void 0 !== e.top && this.scroller.setScrollTop(e.top);
                }),
                (i.prototype.axisStyleAttr = function () {
                    return null != this.axisWidth ? 'style="width:' + this.axisWidth + 'px"' : "";
                }),
                i
            );
        })(t.View);
    m.prototype.usesMinMaxTime = !0;
    var y = (function (e) {
        function r(r) {
            var i = e.call(this, r.el) || this;
            return (i.buildDayRanges = t.memoize(v)), (i.slicer = new S()), (i.timeGrid = r), i;
        }
        return (
            n(r, e),
            (r.prototype.firstContext = function (e) {
                e.calendar.registerInteractiveComponent(this, { el: this.timeGrid.el });
            }),
            (r.prototype.destroy = function () {
                e.prototype.destroy.call(this), this.context.calendar.unregisterInteractiveComponent(this);
            }),
            (r.prototype.render = function (e, t) {
                var r = this.context.dateEnv,
                    i = e.dateProfile,
                    n = e.dayTable,
                    s = (this.dayRanges = this.buildDayRanges(n, i, r)),
                    a = this.timeGrid;
                a.receiveContext(t), a.receiveProps(o({}, this.slicer.sliceProps(e, i, null, t.calendar, a, s), { dateProfile: i, cells: n.cells[0] }), t);
            }),
            (r.prototype.renderNowIndicator = function (e) {
                this.timeGrid.renderNowIndicator(this.slicer.sliceNowDate(e, this.timeGrid, this.dayRanges), e);
            }),
            (r.prototype.buildPositionCaches = function () {
                this.timeGrid.buildPositionCaches();
            }),
            (r.prototype.queryHit = function (e, t) {
                var r = this.timeGrid.positionToHit(e, t);
                if (r) return { component: this.timeGrid, dateSpan: r.dateSpan, dayEl: r.dayEl, rect: { left: r.relativeRect.left, right: r.relativeRect.right, top: r.relativeRect.top, bottom: r.relativeRect.bottom }, layer: 0 };
            }),
            r
        );
    })(t.DateComponent);
    function v(e, t, r) {
        for (var i = [], n = 0, o = e.headerDates; n < o.length; n++) {
            var s = o[n];
            i.push({ start: r.add(s, t.minTime), end: r.add(s, t.maxTime) });
        }
        return i;
    }
    var S = (function (e) {
            function r() {
                return (null !== e && e.apply(this, arguments)) || this;
            }
            return (
                n(r, e),
                (r.prototype.sliceRange = function (e, r) {
                    for (var i = [], n = 0; n < r.length; n++) {
                        var o = t.intersectRanges(e, r[n]);
                        o && i.push({ start: o.start, end: o.end, isStart: o.start.valueOf() === e.start.valueOf(), isEnd: o.end.valueOf() === e.end.valueOf(), col: n });
                    }
                    return i;
                }),
                r
            );
        })(t.Slicer),
        C = (function (e) {
            function i() {
                var r = (null !== e && e.apply(this, arguments)) || this;
                return (r.buildDayTable = t.memoize(E)), r;
            }
            return (
                n(i, e),
                (i.prototype.render = function (t, r) {
                    e.prototype.render.call(this, t, r);
                    var i = this.props,
                        n = i.dateProfile,
                        s = i.dateProfileGenerator,
                        a = r.nextDayThreshold,
                        l = this.buildDayTable(n, s),
                        d = this.splitter.splitProps(t);
                    this.header && this.header.receiveProps({ dateProfile: n, dates: l.headerDates, datesRepDistinctDays: !0, renderIntroHtml: this.renderHeadIntroHtml }, r),
                        this.simpleTimeGrid.receiveProps(o({}, d.timed, { dateProfile: n, dayTable: l }), r),
                        this.simpleDayGrid && this.simpleDayGrid.receiveProps(o({}, d.allDay, { dateProfile: n, dayTable: l, nextDayThreshold: a, isRigid: !1 }), r),
                        this.startNowIndicator(n, s);
                }),
                (i.prototype._renderSkeleton = function (i) {
                    e.prototype._renderSkeleton.call(this, i),
                        i.options.columnHeader && (this.header = new t.DayHeader(this.el.querySelector(".fc-head-container"))),
                        (this.simpleTimeGrid = new y(this.timeGrid)),
                        this.dayGrid && (this.simpleDayGrid = new r.SimpleDayGrid(this.dayGrid));
                }),
                (i.prototype._unrenderSkeleton = function () {
                    e.prototype._unrenderSkeleton.call(this), this.header && this.header.destroy(), this.simpleTimeGrid.destroy(), this.simpleDayGrid && this.simpleDayGrid.destroy();
                }),
                (i.prototype.renderNowIndicator = function (e) {
                    this.simpleTimeGrid.renderNowIndicator(e);
                }),
                i
            );
        })(m);
    function E(e, r) {
        var i = new t.DaySeries(e.renderRange, r);
        return new t.DayTable(i, !1);
    }
    var b = t.createPlugin({
        defaultView: "timeGridWeek",
        views: { timeGrid: { class: C, allDaySlot: !0, slotDuration: "00:30:00", slotEventOverlap: !0 }, timeGridDay: { type: "timeGrid", duration: { days: 1 } }, timeGridWeek: { type: "timeGrid", duration: { weeks: 1 } } },
    });
    (e.AbstractTimeGridView = m), (e.TimeGrid = p), (e.TimeGridSlicer = S), (e.TimeGridView = C), (e.buildDayRanges = v), (e.buildDayTable = E), (e.default = b), Object.defineProperty(e, "__esModule", { value: !0 });
});

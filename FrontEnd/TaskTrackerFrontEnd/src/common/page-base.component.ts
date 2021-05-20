import { Component, OnDestroy, OnInit } from "@angular/core";

@Component({
    selector: "app-page-base",
    templateUrl: "page-base.component.html",
    styleUrls: [],
})
export abstract class PageBaseComponent implements OnInit, OnDestroy {
    isAuthenticated = false;

    constructor() {
        this.isAuthenticated = false;
    }

    ngOnInit(): void {}

    ngOnDestroy(): void {}
}

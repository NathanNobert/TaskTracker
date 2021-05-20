import { Component, EventEmitter, OnInit, Output } from "@angular/core";

@Component({
    selector: "app-home",
    templateUrl: "./home.component.html",
    styleUrls: ["./home.component.css"],
})
export class HomeComponent implements OnInit {
    loggedIn: boolean = false;
    inputText: string;
    constructor() {}

    ngOnInit(): void {}

    myFunction(text) {
        console.log(text);
    }
}

import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";

@Component({
    selector: "simple-text-input",
    templateUrl: "./simple-text-input.component.html",
    styleUrls: ["./simple-text-input.component.css"],
})
export class simpleTextInputComponent implements OnInit {
    @Input() text: string;
    @Input() placeholder: string;
    @Input() label: string;
    @Output() textChanged: EventEmitter<any> = new EventEmitter<any>();

    newItem: string = "test";
    constructor() {}

    ngOnInit(): void {
        console.log(this.text);
    }

    sendText() {
        this.textChanged.emit(this.text);
    }

    sendText2() {
        this.textChanged.emit(this.newItem);
    }
}

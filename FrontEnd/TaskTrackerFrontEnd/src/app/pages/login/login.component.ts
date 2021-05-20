import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { PageBaseComponent } from "src/common/page-base.component";

@Component({
    selector: "app-login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"],
})
export class LoginComponent extends PageBaseComponent implements OnInit {
    username: string;
    password: string;

    constructor(private router: Router) {
        super();
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    onLogin() {
        this.isAuthenticated = true; //temporary until authorization is setup
        this.router.navigateByUrl("/home");
    }
}

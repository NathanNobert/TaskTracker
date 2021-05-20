import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError } from "rxjs/operators";

import { ExceptionHandlerService } from "./exception-handlers.service";
import { User } from "src/models/user.model";
import { AppConfig } from "src/app/app.config";

@Injectable()
export class LoginService {
    constructor(
        private http: HttpClient,
        private errorHandler: ExceptionHandlerService,
        private config: AppConfig
    ) {}

    private apiUrl = `${this.config.getConfig("webApiUrl")}`;

    /**
     * Retrieve a list of users
     * @returns Observable with an Array of Users
     */
    submitLogin(): Observable<Array<User>> {
        let requestUrl = `${this.apiUrl}/submitLogin`;
        console.log(requestUrl);
        return this.http
            .get<Array<User>>(requestUrl)
            .pipe(
                catchError(
                    this.errorHandler.httpExceptionHandler<Array<User>>(
                        `submitLogin Error (url=${requestUrl}`
                    )
                )
            );
    }
}

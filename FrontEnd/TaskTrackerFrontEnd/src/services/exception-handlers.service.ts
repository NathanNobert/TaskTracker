import { Injectable } from "@angular/core";
import { Observable , of } from "rxjs";
import { NGXLogger } from 'ngx-logger';
import { Router } from "@angular/router"; 

@Injectable()
export class ExceptionHandlerService {

    constructor(
        private logger: NGXLogger,
        private router: Router
    ) { }

    /**
     * Handle Http operation that failed.
     * Let the app continue.
     * @param operation - name of the operation that failed
     * @param result - optional value to return as the observable result
     */
    httpExceptionHandler<Promise>(operation = 'operation', result?: any) {
        return (error: any): Observable<any> => {

            if (error.status > 0) {
                // TODO: add the LANID of the current user to the log message
                this.logger.error(operation, error);
            }

            // Redirect to a System Error page
            // TODO: pass error information to the System Error page
            //return Observable.empty<Response>();
            return of(this.router.navigate(["/systemerror"], {queryParams: {status: `${error.status}`, message: `${error.message}`}}));
        };
    }

    /**
     * Handle the Access Denied to a specific API Resource exception
     * @param resource API Resource Controller Name
     */
    accessDeniedExceptionHandler<Promise>(resource: string) {
        return of(this.router.navigate(["/accessdenied"]));
    }

    /**
     * Handle the Invalid Product exception
     * @param trnNum {number} Transaction Number
     * @param pageTitle {string} Page Title
     */
    invalidProductTypeExceptionHandler<Promise>(trnNum: number, pageTitle: string) {
        return of(this.router.navigate(["/invalidproduct"], {queryParams: {trnnum: `${trnNum}`, pagetitle: `${pageTitle}`}}));
    }
}
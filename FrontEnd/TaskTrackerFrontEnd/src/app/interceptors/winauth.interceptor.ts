//#region Change History
// 2020.04.09 - mbew - using sessionStorage instead of localStorage
// 2021.05.05 - mbew - updating the impersonation to include the actualEmpLanId
//#endregion

import { Injectable} from "@angular/core";
import { 
    HttpRequest, 
    HttpHandler, 
    HttpInterceptor, 
    HttpEvent, 
    HttpHeaders
} from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class WinAuthInterceptor implements HttpInterceptor {


    constructor(
    ) { }

    /**
     * Intercept all HTTP Requests and attach any required header values.
     * This is required for Chrome and Firefox, however not for IE.  Chrome 
     * and Firefox ignore sending user credential after the pre-flight request
     * so the header information has to be added prior to resending.
     * @param request Any HTTP Request To Be Intercepted
     * @param next The HTTP Handler To Continue The Request
     */
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let headers = request.headers || new HttpHeaders();
        
        request = request.clone({
            withCredentials: true,
            headers: headers
        });
        
        return next.handle(request);
    }
}
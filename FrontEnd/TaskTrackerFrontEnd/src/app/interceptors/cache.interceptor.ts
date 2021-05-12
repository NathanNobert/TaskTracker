import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpHeaders } from "@angular/common/http";

@Injectable()
export class CacheInterceptor implements HttpInterceptor {

    /**
     * Intercept all HTTP GET requests and prevent requested data from being cached.
     * This is particually needed to Internet Explorer users.
     * @param request Any HTTP Request To Be Intercepted
     * @param next The HTTP Handler To Continue The Request
     */
    intercept(request: HttpRequest<any>, next: HttpHandler) {
        if (request.method === "GET") {
            const httpRequest = request.clone({
                headers: new HttpHeaders({
                    "Cache-Control": "no-cache",
                    "Pragma": "no-cache",
                    "Expires": "Sat, 01 Jan 2000 00:00:00 GMT"
                })                
            });

            return next.handle(httpRequest);
        }

        return next.handle(request);
    }
}
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpInterceptor, HttpEvent} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class NoHttpLoaderInterceptor implements HttpInterceptor {
    /**
     * Intercept all HTTP Requests and remove any "/NoLoad" portion to the URL as it's only required by
     * Chinook to prevent the loading throbber from activating, but is ignored by the API.
     * @param request Any HTTP Request To Be Intercepted
     * @param next The HTTP Handler To Continue The Request
     */
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let modifiedRequest = request.clone({ url: `${request.url.replace(/noload/i, "")}` });
        return next.handle(modifiedRequest);
    }
}
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TestService } from 'src/services/test.service';
import { ExceptionHandlerService } from 'src/services/exception-handlers.service';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppConfig } from './app.config';
import { CacheInterceptor } from './interceptors/cache.interceptor';
import { NoHttpLoaderInterceptor } from './interceptors/no-http-loader.interceptor';
import { WinAuthInterceptor } from './interceptors/winauth.interceptor';
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';
import { MyComponentComponent } from './my-component/my-component.component';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { SystemErrorComponent } from './pages/system-error/system-error.component';
import { routing } from './app.routing';
import { Configuration } from 'msal';
import { MsalAngularConfiguration, MsalModule, MsalService, MSAL_CONFIG, MSAL_CONFIG_ANGULAR } from '@azure/msal-angular';

// checks if the app is running on IE
export const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export function initConfig(config: AppConfig) {
  return () => config.load();
}


@NgModule({
  declarations: [
    AppComponent,
    MyComponentComponent,
    LoginComponent,
    HomeComponent,
    SystemErrorComponent
  ],
  imports: [
    routing,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    LoggerModule.forRoot({
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.DEBUG
  }),
  ],
  providers: [
    TestService,
    ExceptionHandlerService,
    HttpClient,
    AppConfig,
        { 
            provide: APP_INITIALIZER,
            useFactory: initConfig,
            deps: [AppConfig],
            multi: true 
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: CacheInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: WinAuthInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: NoHttpLoaderInterceptor,
            multi: true
        },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

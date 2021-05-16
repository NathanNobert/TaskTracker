import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TestService } from 'src/services/test.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
    title = 'TaskTrackerFrontEnd';
    private destroy: Subject<null> = new Subject();

    constructor(
        private testService: TestService
    ) { }

    ngOnInit() {
        this.checkPreferences();
        this.testService.getUsers().pipe(
            takeUntil(this.destroy)
        ).subscribe(result => {
            console.log(result);
        })
    }

    ngOnDestroy() {
        this.destroy.next(); 
        this.destroy.unsubscribe();
    }

    onToggleDarkMode() {
        let theme = localStorage.getItem('UserTheme');
        if(!theme || theme === 'light') {
            theme = 'dark';
            document.documentElement.classList.add('dark')
        }
        else {
            theme = 'light'
            document.documentElement.classList.remove('dark')
        }
        localStorage.setItem('UserTheme', theme)
        
    }

    private checkPreferences() {
        //load from database the user's preferences 
        let theme = localStorage.getItem('UserTheme');
        document.documentElement.classList.add(theme)
    }
}

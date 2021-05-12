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
}

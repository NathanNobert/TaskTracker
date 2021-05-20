import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
task = 'Default';
output: string;
taskinput = ['test1','test2']
constructor() { }

  ngOnInit(): void {
    console.log("Hello")
  }
myfunction(){
  let name='Riley'
  this.task= this.output
}

}

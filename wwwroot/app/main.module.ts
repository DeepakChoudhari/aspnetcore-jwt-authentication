import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MainComponent } from './main.component';
import { HttpModule } from '@angular/http';

@NgModule({
    imports: [
        BrowserModule,
        HttpModule
    ],
    declarations: [
        MainComponent
    ],
    bootstrap: [ MainComponent ]
})
export class MainModule { }
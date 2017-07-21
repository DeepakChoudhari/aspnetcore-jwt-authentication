import { Component, OnInit } from '@angular/core';
import { Http, Response } from '@angular/http';

@Component({
    'moduleId': module.id,
    'selector': 'main',
    'templateUrl': 'main.component.html'
})
export class MainComponent implements OnInit
{
    userName: string = 'Deepak Choudhari';
    result: any;
    error: any;

    constructor(private http: Http) { }

    ngOnInit() {
        this.http.get('/api/grocerylist').subscribe(response => this.handleServerResponse(response), error => this.handleError(error));
    }

    private handleServerResponse(response: Response) {
        this.result = response.json();
    }

    private handleError(error: any) {
        this.error = error;
    }
}
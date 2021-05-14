import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class AppConfig {

    private config: Object = null;

    constructor(
        private http: HttpClient
    ) { }

    /**
     * Use to get the data found in the env.config.json file (config file)
     */
    public getConfig(key: any) {
        return this.config[key];
    }

    /**
     * This method loads "env.config.json" to get all environment variables
     */
    public load() {
        return new Promise((resolve, reject) => {
            this.http.get("/assets/env/env.config.json").subscribe(
                res => {
                    this.config = res;
                    resolve(true);
                },
                err => {
                    let errMsg = `Error Loading env.config.json - ${err.status} ${err.statusText}`;
                    document.write(errMsg);
                    reject(errMsg);
                }
            );
        });
    }
}
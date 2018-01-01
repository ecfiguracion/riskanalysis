import axios from "axios";
import { store } from "../boot";

export class FormBaseVM {

    public APIUrl: string;
    public IsUseToken: boolean;
    public Model: object;

    constructor(apiUrl: string,isUseToken: boolean) {
        this.IsUseToken = isUseToken;
        this.APIUrl = apiUrl;
        this.Model = {};
    }

    find(id: number) {
        return new Promise((resolve, reject) => {
            if (id > 0) {
                axios.get(this.APIUrl + '/' + id.toString(), {
                        headers: { tokenAuthorization: store.getters.token }
                    })
                    .then(response => {
                        this.Model = response.data;
                        resolve(response);
                    })
                    .catch(error => {
                        reject(error.response);
                    })
            } else {
                resolve(this.Model);
            }
        });
    }

    save() {
        return new Promise((resolve, reject) => {
            axios.post(this.APIUrl, this.Model, {
                    headers: { tokenAuthorization: store.getters.token }
                })
                .then(response => {
                    resolve(response);
                })
                .catch(error => {
                    reject(error.response)
                })
        });
    }
}

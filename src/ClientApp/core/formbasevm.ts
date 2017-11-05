import axios from "axios";

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
        if (id > 0) {
            axios.get(this.APIUrl + '/' + id.toString())
                .then(response => {
                    this.Model = response.data;
                })
        }
    }

    save() {
        return new Promise((resolve, reject) => {
            axios.post(this.APIUrl, this.Model)
                .then(response => {
                    resolve(response);
                })
                .catch(error => {
                    reject(error.response)
                })
        });
    }
}

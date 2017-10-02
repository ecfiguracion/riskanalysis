import axios from 'axios'
import auth from './authenticate'

class VMBase {
    constructor () {        
        // for (var field in data) {
        //     this[field] = data[field]
        // }        

        //Flags
        this.IsUsePagedModel = false
        this.IsUseToken = false

        // Handle the model data
        // By default model is populated
        // If using pagination, pagedmodel is used        
        this.model = { id: 0 }
        this.pagedmodel = {}
        
        //API variables needed for JSON API
        this.API_URL = process.env.API_URL
        this.API_TOKEN_REQUEST = '?api_token='+ auth.getAPIKey()
        
        //Handle ids selected on the grid/list
        this.SelectedIds = []
    }

    post (url) {
        return this.submit('post',this.API_URL + url)
    }

    get (url) {
        return this.submit('get',this.API_URL + url);
    }  

    submit (requestType, url, usePagination) {

        if (this.IsUseToken)
        {
            url = url + this.API_TOKEN_REQUEST
        }

        return new Promise((resolve, reject) => {
            
            //If SelectedIds is not null then use this as data
            var data = this.model;
            if (this.SelectedIds.length > 0) {
                data = this.SelectedIds
            }
            
            axios[requestType](url, data)
            .then(response => {
                if (requestType == 'get') {
                    if (this.IsUsePagedModel) {
                        this.pagedmodel = response.data
                    } else {
                        this.model = response.data
                    }
                }
                resolve(response.data)
            })
            .catch(error => {
                reject(error.response)
            })
        })
    }
}

export default VMBase

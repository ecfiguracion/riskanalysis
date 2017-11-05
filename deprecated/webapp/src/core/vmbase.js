import axios from 'axios'
import API from './constants/api'
import COMMON from './constants/common'

class VMBase {
    constructor () { 
        // Handle the model data    
        this.model = { id: 0 }
        
        // Required flags to be set 
        this.IsUseToken = false
        this.UrlEndPoint = ''
    }

    getUrl (endpoint) {
        if (this.IsUseToken)
            return API.KEYS.URL + this.UrlEndPoint + endpoint + API.KEYS.TOKEN_REQUEST
        else
            return API.KEYS.URL + this.UrlEndPoint + endpoint
    }
    
    find (){
        return this.submit(this.getUrl(''),COMMON.ACTION_TYPES.SUBMIT)
    }
    
    refresh() {
        return this.submit(this.getUrl('/get/' + this.model.id),COMMON.ACTION_TYPES.FINDBYID)
    }

    save() {
        return this.submit(this.getUrl('/save'),COMMON.ACTION_TYPES.SUBMIT)
    }    

    submit (url, action) {
        return new Promise((resolve, reject) => {

            var requestType = 'get'            
            if (action == COMMON.ACTION_TYPES.SUBMIT) {
                requestType = 'post'                    
            }

            axios[requestType](url, this.model)
            .then(response => {
                if (action == COMMON.ACTION_TYPES.FINDBYID) {
                    this.model = response.data
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

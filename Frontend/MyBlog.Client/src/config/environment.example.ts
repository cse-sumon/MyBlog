// Import this in your components to use the API
// Example usage in a component:
// 
// import { environment } from '../../config/environment';
// 
// export class MyComponent {
//   constructor(private http: HttpClient) {
//     console.log(environment.apiUrl); // https://localhost:44390/api
//   }
// }

// Development configuration
export const environment = {
  production: false,
  apiUrl: 'https://localhost:44390/api'
};

/*
  For Production, use environment.prod.ts:
  
  export const environment = {
    production: true,
    apiUrl: 'https://api.yourdomain.com/api'
  };
  
  Update angular.json fileReplacements to use:
  "fileReplacements": [
    {
      "replace": "src/config/environment.ts",
      "with": "src/config/environment.prod.ts"
    }
  ]
*/

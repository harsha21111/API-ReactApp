import React, { Component } from 'react';
import axios from 'axios';


const apiUrl = 'https://localhost:44311/product';

export class ProductsList extends Component {
    static displayName = ProductsList.name;
    constructor(props) {
        super(props);

        this.state = {
            product:[],
            products: [],
            productId: Number,
             loading: true,
            response: {},
            error:null  
        };
    }
    handleChange = (e) => {
        this.setState({[e.target.name]:e.target.value})
     }
    componentDidMount(){  
        axios.get(apiUrl + '').then(response => response.data).then(  
             (result)=>{  
                 this.setState({  
                    products:result  
                 });  
             },  
             (error)=>{  
                 this.setState({error});  
             }  
         )  
     }; 
    

    deleteProduct(productId) {  
        const { products } = this.state;  
        console.log(this.state);   
       axios.delete(apiUrl + '/' + this.state.productId).then(result=>{  
         alert(result.data);  
          this.setState({  
            response:result,  
            products:products.filter(p=>p.ProductId !== productId)  
          });  
        });  
      };

      getProductById(productId) {  
        const { products } = this.state;  
        console.log(this.state);   
       axios.get(apiUrl + '/' + this.state.productId).then(result=>{  
         console.log(result.data);  
          this.setState({  
            response:result,  
            products:products.filter(p=>p.ProductId !== productId)  
          });  
        });  
      };

      insertProduct(product){
        
        const params = JSON.stringify(JSON.parse(product));
        axios.post(apiUrl, params,{
            "headers": {
            "content-type": "application/json",
            },
            });
            console.log(params);
        };

        updateProduct(product){
        
            const params = JSON.stringify(JSON.parse(product));
            axios.put(apiUrl, params,{
                "headers": {
                "content-type": "application/json",
                },
                });
                console.log(params);
            };

    render() {  
  
        return (  
            <section>  
                <h1>Products List</h1>  
                <div>
                    <table>
                        <thead><tr><th>Product Id</th><th>Product Name</th><th>Product Category</th><th>Product Price</th></tr></thead>
                        <tbody>
                            {
                                this.state.products.map((p, index) => {
                                    return <tr key={index}><td>{p.productId}</td><td> {p.productName}</td><td>{p.productCategory}</td><td>{p.productPrice}</td>
                                    </tr>;
                                })
                            }
                        </tbody>
                    </table>
                    <input type="text" name="productId" onChange={this.handleChange} />
                    <button  onClick={() => this.deleteProduct(this.state.productId)}> Delete </button>
                    <button  onClick={() => this.getProductById(this.state.productId)}> GetBYID </button>
                    
                </div> 
                <br></br>
                <div>
                    <textarea name="product" onChange={this.handleChange}></textarea>
                    <button  onClick={() => this.insertProduct(this.state.product)}> Insert </button>
                    <button  onClick={() => this.updateProduct(this.state.product)}> Update </button>

                </div>
                
            </section>  
        )  
    }  
}  

export default ProductsList;
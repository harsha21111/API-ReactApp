import React, { Component } from 'react';
 
export class ProductsList extends Component {
    static displayName = ProductsList.name;
    constructor(props) {
        super(props);

        this.state = {
            ProductData: [], loading: true
        };
    }

    componentDidMount() {console.log("am here 1")
        //await fetch('https://localhost:44311/product')
        //    .then(response => response.json())
        //    .then(ProductData => function () {

        //        this.setState({ ProductData });
        //    }

        //    );
        this.test();
    }

    async test() {
        const response = await fetch('https://localhost:44311/product');
        const data = await response.json();
        this.setState({ ProductData: data, loading: false });
        console.log(this.state.ProductData);
    }

    render() {  
  
        return (  
            <section>  
                <h1>Products List</h1>  
                <div>
                    <table>
                        <thead><tr><th>Product Id</th><th>Product Name</th><th>Product Category</th><th>Product Price</th></tr></thead>
                        <tbody>
                            {
                                this.state.ProductData.map((p, index) => {
                                    return <tr key={index}><td>{p.productId}</td><td> {p.productName}</td><td>{p.productCategory}</td><td>{p.productPrice}</td></tr>;
                                })
                            }
                        </tbody>
                    </table>
                </div> 
  
  
            </section>  
        )  
    }  
}  

export default ProductsList;
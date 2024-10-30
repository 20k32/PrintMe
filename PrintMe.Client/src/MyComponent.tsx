import { useState, useEffect } from 'react';
import axios from 'axios';

interface Product {
    value: string
}
const MyComponent = () => {
    const [myDataBag, setData] = useState<Product | null>(null);

    useEffect(() => {
        // Fetch data from the API
        try {
            axios.get<Product>('/api/Test/TestGetData')
                .then(response => {
                        setData(response.data);
                    })
                .catch(error => console.log(error.message));
        }
        catch(error) {
            console.error('Error fetching the products:', error);
        }
    });
    
    return (
        <div>
             <h1>{myDataBag?.value ?? "empty"}</h1>
             <h1>World</h1>
        </div>
    );
};

export default MyComponent;

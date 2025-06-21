import {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext } from "../../context";
import { Layout } from "../../components";
import { Navigate } from "react-router-dom";
import Stack from '@mui/material/Stack';
import { UserAuthenticationService } from "../../services";
import { User } from "oidc-client";

export interface PageTemplateProps{
    children:any
}

function PageTemplate(props: PageTemplateProps) {
    const [user, setUser] = useState<User|null>(null);
    const [redirect, setRedirect] = useState(false);
    const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext);  


    useEffect(() => {
    
        async function getUserAsync() {
          const user = await userAuthenticationService?.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
           if(user){
            setUser(user);
          }else{
            setRedirect(true);
          }
        });
       
      }, []);


      return (
        <>
            {!user ?
                <>
                    {redirect ? 
                        <Navigate to="login"/>: <div></div>
                    }
                </>
                :
                <Layout>
                    
                    <Stack sx={{width:'100%', height:'100%'}} direction='column' alignItems='center' gap={2}>
                        {props.children}
                    </Stack>
                </Layout>
            }
        </>
      )
}

export default PageTemplate;
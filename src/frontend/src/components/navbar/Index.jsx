import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { Component } from 'react'
import { IconButton, Stack } from '@mui/material';
import { Link } from 'react-router-dom';
import CategoryOutlinedIcon from '@mui/icons-material/CategoryOutlined';
import AddShoppingCartOutlinedIcon from '@mui/icons-material/AddShoppingCartOutlined';
import CreditCardOutlinedIcon from '@mui/icons-material/CreditCardOutlined';
import InventoryOutlinedIcon from '@mui/icons-material/InventoryOutlined';
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';

export class NavBar extends Component {
    render() {
        return (
            <Box sx={{ flexGrow: 1 }}>
                <AppBar position="static">
                    <Toolbar sx={{ bgcolor: "black" }}>
                        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                            Not So Simple Ecommerce
                        </Typography>
                        <Stack direction="row">
                            <Link to="/product">
                                <Button color="inherit" endIcon={<CategoryOutlinedIcon />}>Produto</Button>
                            </Link>
                            <Link to="/stock">
                                <Button color="inherit" endIcon={<InventoryOutlinedIcon />}>Estoque</Button>
                            </Link>
                            <Link to="/order">
                                <Button color="inherit" endIcon={<CreditCardOutlinedIcon />}>Ordem</Button>
                            </Link>
                            <Link to="/report">
                                <Button color="inherit" endIcon={<CategoryOutlinedIcon />}>Relatório</Button>
                            </Link>
                            <Link to="/login">
                                <Button color="inherit"endIcon={<LoginOutlinedIcon />}>Login</Button>
                            </Link>
                        </Stack>
                    </Toolbar>
                </AppBar>
            </Box>
        )
    }
}

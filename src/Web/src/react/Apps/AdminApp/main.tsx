import React from 'react';
import ReactDOM from 'react-dom/client';
import { Provider } from 'react-redux';
import { createBrowserRouter, RouterProvider, Navigate } from 'react-router-dom';
import { store } from '@/redux/store';
import { AdminLayout } from '@/Apps/Common/AdminLayout';
import { ManageBlogsPage } from './Pages/ManageBlogs';
import { EditBlogPage } from './Pages/EditBlog';
import { ManagePostsPage } from './Pages/ManagePosts';
import { EditPostPage } from './Pages/EditPost';
import { ManageCommentsPage } from './Pages/ManageComments';
import { ManageListsPage } from './Pages/ManageLists';
import { EditListPage } from './Pages/EditList';
import { SiteInfoPage } from './Pages/SiteInfo';
import { ManageUsersPage } from './Pages/ManageUsers';
import { EditUserPage } from './Pages/EditUser';
import '@/index.css';

const router = createBrowserRouter([
    {
        path: '/Admin/App',
        element: <AdminLayout />,
        children: [
            {
                index: true,
                element: <Navigate to="/Admin/App/ManageBlogs" replace />,
            },
            {
                path: 'SiteInfo',
                element: <SiteInfoPage />,
            },
            {
                path: 'ManageBlogs',
                element: <ManageBlogsPage />,
            },
            {
                path: 'EditBlog/:id',
                element: <EditBlogPage />,
            },
            {
                path: 'ManagePosts/:blogSubFolder',
                element: <ManagePostsPage />,
            },
            {
                path: 'EditPost/:blogSubFolder/:id',
                element: <EditPostPage />,
            },
            {
                path: 'ManageComments/:blogSubFolder',
                element: <ManageCommentsPage />,
            },
            {
                path: 'ManageLists/:blogSubFolder',
                element: <ManageListsPage />,
            },
            {
                path: 'EditList/:blogSubFolder/:id',
                element: <EditListPage />,
            },
            {
                path: 'ManageUsers',
                element: <ManageUsersPage />,
            },
            {
                path: 'EditUser/:id',
                element: <EditUserPage />,
            },
        ],
    },
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <Provider store={store}>
            <RouterProvider router={router} />
        </Provider>
    </React.StrictMode>
);

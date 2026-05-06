import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { fetchBlogs } from '@/redux/BlogSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Table } from '@/components/Table';
import { Button } from '@/components/Button';
import { IBlog } from '@/Models/IBlog';

export const ManageBlogsPage: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const { blogs, loading, error } = useSelector((state: RootState) => state.blogs);

    useEffect(() => {
        dispatch(fetchBlogs());
    }, [dispatch]);

    const columns = [
        { header: 'Name', key: 'Name', render: (blog: IBlog) => <Link to={`/Admin/App/EditBlog/${blog.Id}`} className="text-blue-600 hover:underline">{blog.Name}</Link> },
        { header: 'Description', key: 'Description' },
        { header: 'SubFolder', key: 'SubFolder' },
        { 
            header: 'Actions', 
            key: 'Actions', 
            render: (blog: IBlog) => (
                <div className="flex space-x-4">
                    <Link to={`/Admin/App/ManagePosts/${blog.SubFolder}`} className="text-blue-600 hover:underline">Manage Posts</Link>
                    <Link to={`/Admin/App/ManageComments/${blog.SubFolder}`} className="text-blue-600 hover:underline">Manage Comments</Link>
                </div>
            )
        },
    ];

    if (loading) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900">Manage Blogs</h1>
                <Link to="/Admin/App/EditBlog/-1">
                    <Button variant="primary">Add Blog</Button>
                </Link>
            </div>
            <Table data={blogs} columns={columns} keyField="Id" />
        </div>
    );
};

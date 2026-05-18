import React, { useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchListsByBlog, deleteList } from '@/redux/BlogListSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Table } from '@/components/Table';
import { Button } from '@/components/Button';
import { IBlogList } from '@/types/blog-list.types';

export const ManageListsPage: React.FC = () => {
    const { blogSubFolder } = useParams<{ blogSubFolder: string }>();
    const dispatch = useDispatch<AppDispatch>();
    const { lists, loading, error } = useSelector((state: RootState) => state.blogLists);

    useEffect(() => {
        if (blogSubFolder) {
            dispatch(fetchListsByBlog(blogSubFolder));
        }
    }, [blogSubFolder, dispatch]);

    const handleDelete = async (id: number) => {
        if (blogSubFolder && window.confirm('Are you sure you want to delete this list?')) {
            await dispatch(deleteList({ blogSubFolder, id }));
        }
    };

    const columns = [
        { 
            header: 'Name', 
            key: 'Name', 
            render: (list: IBlogList) => (
                <Link to={`/Admin/App/EditList/${blogSubFolder}/${list.Id}`} className="text-blue-600 hover:underline">
                    {list.Name}
                </Link>
            ) 
        },
        { header: 'Ordered', key: 'ShowOrdered', render: (list: IBlogList) => list.ShowOrdered ? 'Yes' : 'No' },
        { header: 'Items', key: 'ItemsCount', render: (list: IBlogList) => list.Items?.length || 0 },
        { 
            header: 'Actions', 
            key: 'Actions', 
            render: (list: IBlogList) => (
                <div className="flex space-x-2">
                    <Button 
                        variant="danger" 
                        size="sm" 
                        onClick={() => handleDelete(list.Id)}
                    >
                        Delete
                    </Button>
                </div>
            ) 
        },
    ];

    if (loading) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900">Manage Lists: {blogSubFolder}</h1>
                <div className="flex space-x-2">
                    <Link to="/Admin/App/ManageBlogs">
                        <Button variant="secondary">Back to Blogs</Button>
                    </Link>
                    <Link to={`/Admin/App/EditList/${blogSubFolder}/-1`}>
                        <Button variant="primary">Add List</Button>
                    </Link>
                </div>
            </div>
            <Table data={lists} columns={columns} keyField="Id" />
        </div>
    );
};


import React, { useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchPostsByBlog } from '@/redux/BlogPostSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Table } from '@/components/Table';
import { Button } from '@/components/Button';
import { IBlogPost } from '@/Models/IBlogPost';

export const ManagePostsPage: React.FC = () => {
    const { blogSubFolder } = useParams<{ blogSubFolder: string }>();
    const dispatch = useDispatch<AppDispatch>();
    const { posts, loading, error } = useSelector((state: RootState) => state.blogPosts);

    useEffect(() => {
        if (blogSubFolder) {
            dispatch(fetchPostsByBlog(blogSubFolder));
        }
    }, [blogSubFolder, dispatch]);

    const columns = [
        { header: 'Title', key: 'Title', render: (post: IBlogPost) => <Link to={`/Admin/App/EditPost/${blogSubFolder}/${post.Id}`} className="text-blue-600 hover:underline">{post.Title}</Link> },
        { header: 'Date Posted', key: 'DatePosted', render: (post: IBlogPost) => new Date(post.DatePosted).toLocaleDateString() },
        { header: 'Published', key: 'IsPublished', render: (post: IBlogPost) => post.IsPublished ? 'Yes' : 'No' },
        { header: 'Views', key: 'TimesViewed' },
        { header: 'Comments', key: 'CommentCount' },
    ];

    if (loading) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900">Manage Posts: {blogSubFolder}</h1>
                <div className="flex space-x-2">
                    <Link to="/Admin/App/ManageBlogs">
                        <Button variant="secondary">Back to Blogs</Button>
                    </Link>
                    <Link to={`/Admin/App/EditPost/${blogSubFolder}/-1`}>
                        <Button variant="primary">Add Post</Button>
                    </Link>
                </div>
            </div>
            <Table data={posts} columns={columns} keyField="Id" />
        </div>
    );
};

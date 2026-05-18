import React, { useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchCommentsByBlog, updateCommentStatus, deleteComment } from '@/redux/CommentSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Table } from '@/components/Table';
import { Button } from '@/components/Button';
import { IComment } from '@/types/comment.types';

export const ManageCommentsPage: React.FC = () => {
    const { blogSubFolder } = useParams<{ blogSubFolder: string }>();
    const dispatch = useDispatch<AppDispatch>();
    const { comments, loading, error } = useSelector((state: RootState) => state.comments);

    useEffect(() => {
        if (blogSubFolder) {
            dispatch(fetchCommentsByBlog({ blogSubFolder }));
        }
    }, [blogSubFolder, dispatch]);

    const handleStatusUpdate = (comment: IComment, newStatus: string) => {
        if (blogSubFolder) {
            dispatch(updateCommentStatus({ 
                blogSubFolder, 
                postId: comment.BlogPostId, 
                commentId: comment.Id, 
                newState: newStatus 
            }));
        }
    };

    const handleDelete = (comment: IComment) => {
        if (blogSubFolder && window.confirm('Are you sure you want to delete this comment?')) {
            dispatch(deleteComment({ 
                blogSubFolder, 
                postId: comment.BlogPostId, 
                commentId: comment.Id 
            }));
        }
    };

    const columns = [
        { header: 'Author', key: 'AuthorName' },
        { header: 'Email', key: 'AuthorEmail' },
        { header: 'Comment', key: 'Text', render: (comment: IComment) => <div className="max-w-xs truncate" title={comment.Text}>{comment.Text}</div> },
        { header: 'Date', key: 'DatePosted', render: (comment: IComment) => new Date(comment.DatePosted).toLocaleDateString() },
        { header: 'Status', key: 'Status', render: (comment: IComment) => {
            const statusMap: Record<number, string> = { 0: 'Pending', 1: 'Approved', 2: 'Spam', 3: 'Deleted' };
            return statusMap[comment.Status] || 'Unknown';
        }},
        { header: 'Actions', key: 'Actions', render: (comment: IComment) => (
            <div className="flex space-x-2">
                {comment.Status !== 1 && (
                    <Button variant="primary" onClick={() => handleStatusUpdate(comment, 'Approved')}>Approve</Button>
                )}
                {comment.Status !== 2 && (
                    <Button variant="secondary" onClick={() => handleStatusUpdate(comment, 'Spam')}>Spam</Button>
                )}
                <Button variant="secondary" onClick={() => handleDelete(comment)}>Delete</Button>
            </div>
        )}
    ];

    if (loading) return <div>Loading...</div>;
    if (error) return <div className="text-red-600">Error: {error}</div>;

    return (
        <div className="bg-white shadow rounded-lg p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900">Manage Comments: {blogSubFolder}</h1>
                <Link to="/Admin/App/ManageBlogs">
                    <Button variant="secondary">Back to Blogs</Button>
                </Link>
            </div>
            <Table data={comments} columns={columns} keyField="Id" />
        </div>
    );
};


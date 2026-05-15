import React, { useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { fetchPostsByBlog, deletePost } from '@/redux/BlogPostSlice';
import { RootState, AppDispatch } from '@/redux/store';
import { Table } from '@/components/Table';
import { Button } from '@/components/Button';
import { IBlogPost } from '@/Models/IBlogPost';

export const ManagePostsPage: React.FC = () => {
    const { blogSubFolder } = useParams<{ blogSubFolder: string }>();
    const dispatch = useDispatch<AppDispatch>();
    const { posts, loading, error } = useSelector((state: RootState) => state.blogPosts);
    const [currentPage, setCurrentPage] = React.useState(1);
    const [sortConfig, setSortConfig] = React.useState<{ key: string, direction: 'asc' | 'desc' } | null>({ key: 'DateCreated', direction: 'desc' });
    const itemsPerPage = 10;

    useEffect(() => {
        if (blogSubFolder) {
            dispatch(fetchPostsByBlog(blogSubFolder));
        }
    }, [blogSubFolder, dispatch]);

    const handleDelete = async (id: number) => {
        if (blogSubFolder && window.confirm('Are you sure you want to delete this post?')) {
            await dispatch(deletePost({ blogSubFolder, id }));
        }
    };

    const handleSort = (key: string) => {
        let direction: 'asc' | 'desc' = 'asc';
        if (sortConfig && sortConfig.key === key && sortConfig.direction === 'asc') {
            direction = 'desc';
        }
        setSortConfig({ key, direction });
        setCurrentPage(1); // Reset to first page when sorting changes
    };

    const sortedPosts = React.useMemo(() => {
        const items = [...posts];
        if (sortConfig !== null) {
            items.sort((a, b) => {
                const aValue = a[sortConfig.key as keyof IBlogPost];
                const bValue = b[sortConfig.key as keyof IBlogPost];

                if (aValue === undefined || bValue === undefined) return 0;

                if (aValue < bValue) {
                    return sortConfig.direction === 'asc' ? -1 : 1;
                }
                if (aValue > bValue) {
                    return sortConfig.direction === 'asc' ? 1 : -1;
                }
                return 0;
            });
        }
        return items;
    }, [posts, sortConfig]);

    const totalPages = Math.ceil(sortedPosts.length / itemsPerPage);
    const paginatedPosts = React.useMemo(() => {
        const startIndex = (currentPage - 1) * itemsPerPage;
        return sortedPosts.slice(startIndex, startIndex + itemsPerPage);
    }, [sortedPosts, currentPage]);

    const columns = [
        { header: 'Title', key: 'Title', render: (post: IBlogPost) => <Link to={`/Admin/App/EditPost/${blogSubFolder}/${post.Id}`} className="text-blue-600 hover:underline">{post.Title}</Link> },
        { header: 'Date Created', key: 'DateCreated', sortable: true, render: (post: IBlogPost) => new Date(post.DateCreated).toLocaleDateString() },
        { header: 'Date Posted', key: 'DatePosted', sortable: true, render: (post: IBlogPost) => new Date(post.DatePosted).toLocaleDateString() },
        { header: 'Published', key: 'IsPublished', render: (post: IBlogPost) => post.IsPublished ? 'Yes' : 'No' },
        { header: 'Views', key: 'TimesViewed', sortable: true },
        { header: 'Comments', key: 'CommentCount', sortable: true },
        { 
            header: 'Actions', 
            key: 'Actions', 
            render: (post: IBlogPost) => (
                <div className="flex space-x-2">
                    {!post.IsPublished && (
                        <Button 
                            variant="danger" 
                            size="sm" 
                            onClick={() => handleDelete(post.Id)}
                        >
                            Delete
                        </Button>
                    )}
                </div>
            ) 
        },
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
            <Table 
                data={paginatedPosts} 
                columns={columns} 
                keyField="Id" 
                onSort={handleSort}
                sortConfig={sortConfig}
            />
            
            {totalPages > 1 && (
                <div className="flex justify-between items-center mt-6">
                    <div className="text-sm text-gray-700">
                        Showing {(currentPage - 1) * itemsPerPage + 1} to {Math.min(currentPage * itemsPerPage, sortedPosts.length)} of {sortedPosts.length} posts
                    </div>
                    <div className="flex space-x-2">
                        <Button 
                            variant="secondary" 
                            size="sm" 
                            disabled={currentPage === 1}
                            onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}
                        >
                            Previous
                        </Button>
                        <div className="flex space-x-1">
                            {Array.from({ length: totalPages }, (_, i) => i + 1).map(page => (
                                <button
                                    key={page}
                                    className={`px-3 py-1 text-sm font-medium rounded-md ${currentPage === page ? 'bg-blue-600 text-white' : 'text-gray-700 hover:bg-gray-100'}`}
                                    onClick={() => setCurrentPage(page)}
                                >
                                    {page}
                                </button>
                            ))}
                        </div>
                        <Button 
                            variant="secondary" 
                            size="sm" 
                            disabled={currentPage === totalPages}
                            onClick={() => setCurrentPage(prev => Math.min(prev + 1, totalPages))}
                        >
                            Next
                        </Button>
                    </div>
                </div>
            )}
        </div>
    );
};

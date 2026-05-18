import RestClient from './RestClient';
import { IBlogList, IBlogListItem } from '@/types/blog-list.types';

class BlogListRepository {
    public async getByBlog(blogSubFolder: string): Promise<IBlogList[]> {
        try {
            return await RestClient.get<IBlogList[]>(`/api/Blog/${blogSubFolder}/Lists`);
        } catch (error) {
            console.error(`Failed to fetch lists for blog ${blogSubFolder}:`, error);
            throw new Error('Could not retrieve blog lists. Please try again later.');
        }
    }

    public async getById(blogSubFolder: string, id: number): Promise<IBlogList> {
        try {
            return await RestClient.get<IBlogList>(`/api/Blog/${blogSubFolder}/List/${id}`);
        } catch (error) {
            console.error(`Failed to fetch list ${id} for blog ${blogSubFolder}:`, error);
            throw new Error('Could not retrieve the blog list. Please try again later.');
        }
    }

    public async save(blogSubFolder: string, list: IBlogList): Promise<IBlogList> {
        try {
            const input = {
                Name: list.Name,
                ShowOrdered: list.ShowOrdered
            };

            if (list.Id && list.Id > 0) {
                return await RestClient.put<IBlogList>(`/api/Blog/${blogSubFolder}/List/${list.Id}`, input);
            } else {
                return await RestClient.post<IBlogList>(`/api/Blog/${blogSubFolder}/List`, input);
            }
        } catch (error) {
            console.error(`Failed to save list for blog ${blogSubFolder}:`, error);
            throw new Error('Could not save the blog list. Please check your input and try again.');
        }
    }

    public async delete(blogSubFolder: string, id: number): Promise<boolean> {
        try {
            return await RestClient.delete<boolean>(`/api/Blog/${blogSubFolder}/List/${id}`);
        } catch (error) {
            console.error(`Failed to delete list ${id} for blog ${blogSubFolder}:`, error);
            throw new Error('Could not delete the blog list.');
        }
    }

    public async saveItem(blogSubFolder: string, listId: number, item: IBlogListItem): Promise<IBlogList> {
        try {
            const input = {
                Name: item.Name,
                RelatedLink: item.RelatedLink,
                DisplayOrder: item.DisplayOrder
            };

            if (item.Id && item.Id > 0) {
                return await RestClient.put<IBlogList>(`/api/Blog/${blogSubFolder}/List/${listId}/Item/${item.Id}`, input);
            } else {
                return await RestClient.post<IBlogList>(`/api/Blog/${blogSubFolder}/List/${listId}/Item`, input);
            }
        } catch (error) {
            console.error(`Failed to save item for list ${listId} in blog ${blogSubFolder}:`, error);
            throw new Error('Could not save the list item.');
        }
    }

    public async deleteItem(blogSubFolder: string, listId: number, itemId: number): Promise<IBlogList> {
        try {
            return await RestClient.delete<IBlogList>(`/api/Blog/${blogSubFolder}/List/${listId}/Item/${itemId}`);
        } catch (error) {
            console.error(`Failed to delete item ${itemId} from list ${listId} in blog ${blogSubFolder}:`, error);
            throw new Error('Could not delete the list item.');
        }
    }
}

export default new BlogListRepository();

